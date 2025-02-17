using RQL2Expression.Core.Abstract;
using RQL2Expression.Core.Models;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace RQL2Expression.Core.Service
{
    public class RqlToExpressionService : IRqlToExpressionService
    {
        private readonly IRqlAttributeMapper _attributeMapper;

        public RqlToExpressionService(IRqlAttributeMapper attributeMapper)
        {
            _attributeMapper = attributeMapper;
        }

        /// <summary>
        /// Преобразует RQL-запрос в дерево выражений для фильтрации Account.
        /// </summary>
        /// <param name="rql">RQL-запрос.</param>
        /// <returns>Выражение для фильтрации.</returns>
        public Expression<Func<Account, bool>> ParseRqlToExpression(string rql, ParameterExpression parameter = null)
        {
            parameter = parameter ?? Expression.Parameter(typeof(Account), "a");

            rql = rql.Replace(" ", string.Empty).Replace("\n", string.Empty);

            if (rql.StartsWith("and("))
            {
                var innerConditions = GetInnerConditions(rql, "and");
                var combinedExpression = CombineConditions(innerConditions, parameter, Expression.AndAlso);
                return Expression.Lambda<Func<Account, bool>>(combinedExpression, parameter);
            }

            if (rql.StartsWith("or("))
            {
                var innerConditions = GetInnerConditions(rql, "or");
                var combinedExpression = CombineConditions(innerConditions, parameter, Expression.OrElse);
                return Expression.Lambda<Func<Account, bool>>(combinedExpression, parameter);
            }

            if (rql.StartsWith("in("))
            {
                var parts = rql.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    var field = parts[1];
                    var values = parts[2].Split(',');

                    var propertyName = _attributeMapper.MapToPropertyName(field);
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        return a => true;
                    }

                    var property = Expression.Property(parameter, propertyName);

                    Expression combinedExpression = null;
                    foreach (var value in values)
                    {
                        var constant = Expression.Constant(Convert.ChangeType(value, property.Type));
                        var equalExpression = Expression.Equal(property, constant);

                        if (combinedExpression == null)
                        {
                            combinedExpression = equalExpression;
                        }
                        else
                        {
                            combinedExpression = Expression.OrElse(combinedExpression, equalExpression);
                        }
                    }

                    return Expression.Lambda<Func<Account, bool>>(combinedExpression, parameter);
                }
            }

            if (rql.StartsWith("eq("))
            {
                var parts = rql.Split(new[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 3)
                {
                    var field = parts[1];
                    var value = parts[2];

                    var propertyName = _attributeMapper.MapToPropertyName(field);
                    if (string.IsNullOrEmpty(propertyName))
                    {
                        return a => true;
                    }

                    var property = Expression.Property(parameter, propertyName);

                    if (value.Contains("*"))
                    {
                        if (value.StartsWith("*") && value.EndsWith("*"))
                        {
                            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                            var containsExpression = Expression.Call(property, containsMethod, Expression.Constant(value.Trim('*')));
                            return Expression.Lambda<Func<Account, bool>>(containsExpression, parameter);
                        }
                        else if (value.StartsWith("*"))
                        {
                            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                            var endsWithExpression = Expression.Call(property, endsWithMethod, Expression.Constant(value.Trim('*')));
                            return Expression.Lambda<Func<Account, bool>>(endsWithExpression, parameter);
                        }
                        else if (value.EndsWith("*"))
                        {
                            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                            var startsWithExpression = Expression.Call(property, startsWithMethod, Expression.Constant(value.Trim('*')));
                            return Expression.Lambda<Func<Account, bool>>(startsWithExpression, parameter);
                        }
                    }
                    else
                    {
                        var constant = Expression.Constant(Convert.ChangeType(value, property.Type));
                        var equalExpression = Expression.Equal(property, constant);
                        return Expression.Lambda<Func<Account, bool>>(equalExpression, parameter);
                    }
                }
            }

            return a => true;
        }

        /// <summary>
        /// Метод для получения внутренних условий
        /// </summary>
        /// <param name="rql"></param>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        private List<string> GetInnerConditions(string rql, string operatorName)
        {
            var conditions = new List<string>();
            var startIndex = operatorName.Length + 1;
            var endIndex = rql.LastIndexOf(')');
            var innerRql = rql.Substring(startIndex, endIndex - startIndex);

            var stack = 0;
            var start = 0;

            for (var i = 0; i < innerRql.Length; i++)
            {
                if (innerRql[i] == '(') stack++;
                if (innerRql[i] == ')') stack--;
                if (innerRql[i] == ',' && stack == 0)
                {
                    conditions.Add(innerRql.Substring(start, i - start));
                    start = i + 1;
                }
            }

            if (start < innerRql.Length)
            {
                conditions.Add(innerRql.Substring(start));
            }

            return conditions;
        }

        /// <summary>
        /// Метод для комбинирования условий
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="parameter"></param>
        /// <param name="combineFunc"></param>
        /// <returns></returns>
        private Expression CombineConditions(List<string> conditions, ParameterExpression parameter, Func<Expression, Expression, Expression> combineFunc)
        {
            Expression combinedExpression = null;

            foreach (var condition in conditions)
            {
                var conditionExpression = ParseRqlToExpression(condition, parameter).Body;

                if (combinedExpression == null)
                {
                    combinedExpression = conditionExpression;
                }
                else
                {
                    combinedExpression = combineFunc(combinedExpression, conditionExpression);
                }
            }

            return combinedExpression;
        }

        /// <summary>
        /// Метод для получения значения limit
        /// </summary>
        /// <param name="rql"></param>
        /// <returns></returns>
        private int GetLimitValue(string rql)
        {
            const int DefaultLimitValue = 1;

            var limitMatch = Regex.Match(rql, @"limit\((\d+)\)");
            if (limitMatch.Success && int.TryParse(limitMatch.Groups[1].Value, out var limit))
            {
                return limit;
            }

            return DefaultLimitValue;
        }
    }
}
