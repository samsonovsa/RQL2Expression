namespace RQL2Expression.Core.Options
{
    public class RqlAttributeOptions
    {
        public const string OptionName = "RqlAttribute";

        public int DefaultLimitValue { get; set; }

        public string PhoneAttributeName { get; set; }

        public string MailAttributeName { get; set; }

        public string UidAttributeName { get; set; }

        public string UserAuthPhoneAttributeName { get; set; }

        public string ModeAttributeName { get; set; }
    }
}
