namespace RQL2Expression.Core.Models
{
    public class Account
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; }

        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public DateTime? LastLoginDate { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string EmailAddress { get; set; }

        public bool EmailVerified { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneVerified { get; set; }


        public ICollection<AccountHistory> History { get; set; }
    }
}
