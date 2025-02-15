namespace RQL2Expression.Core.Models
{
    public class AccountHistory
    {
        public int Id { get; set; }

        public DateTime ChangeDate { get; set; }

        public string ParameterName { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }

        public string ChangeSource { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }
    }
}