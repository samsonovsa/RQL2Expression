using Microsoft.Extensions.Options;
using RQL2Expression.Core.Abstract;
using RQL2Expression.Core.Models;
using RQL2Expression.Core.Options;

namespace RQL2Expression.Core.Service
{
    public class RqlAttributeMapper : IRqlAttributeMapper
    {
        private readonly Dictionary<string, string> _attributeMappings;

        public RqlAttributeMapper(IOptions<RqlAttributeOptions> rqlSettings)
        {
            var rqlAttributeSettings = rqlSettings?.Value ?? throw new ArgumentNullException(nameof(rqlSettings));

            // Initialize a dictionary for mapping RQL attributes to model properties
            _attributeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { rqlAttributeSettings.MailAttributeName, nameof(Account.EmailAddress) },
                { rqlAttributeSettings.PhoneAttributeName, nameof(Account.PhoneNumber) },
                { rqlAttributeSettings.UidAttributeName, nameof(Account.Id) },
                { rqlAttributeSettings.UserAuthPhoneAttributeName, nameof(Account.PhoneNumber) },
            };
        }

        public string MapToPropertyName(string rqlAttribute)
        {
            if (_attributeMappings.TryGetValue(rqlAttribute, out var propertyName))
            {
                return propertyName;
            }

            return string.Empty;
        }
    }
}
