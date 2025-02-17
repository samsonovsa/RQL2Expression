using RQL2Expression.Core.Models;
using System.Linq.Expressions;

namespace RQL2Expression.Core.Abstract
{
    public interface IRqlToExpressionService
    {
        Expression<Func<Account, bool>> ParseRqlToExpression(string rql, ParameterExpression parameter = null);
    }
}
