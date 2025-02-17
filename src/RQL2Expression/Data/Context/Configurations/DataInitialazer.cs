using Microsoft.EntityFrameworkCore;
using RQL2Expression.Core.Models;

namespace RQL2Expression.Data.Context.Configurations
{
    public static class DataInitialazer
    {
        public static void Init(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = AccountStatus.Active,
                    LastLoginDate = DateTime.UtcNow.AddDays(-10),
                    LastName = "Doe",
                    FirstName = "John",
                    MiddleName = "Michael",
                    EmailAddress = "john.doe@example.com",
                    EmailVerified = true,
                    PhoneNumber = "+1234567890",
                    PhoneVerified = true
                },
                new Account
                {
                    Id = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Status = AccountStatus.Inactive,
                    LastLoginDate = DateTime.UtcNow.AddDays(-20),
                    LastName = "Smith",
                    FirstName = "Jane",
                    MiddleName = "Anne",
                    EmailAddress = "jane.smith@example.com",
                    EmailVerified = false,
                    PhoneNumber = "+0987654321",
                    PhoneVerified = false
                }
            );

            modelBuilder.Entity<AccountHistory>().HasData(
                new AccountHistory
                {
                    Id = 1,
                    ChangeDate = DateTime.UtcNow.AddDays(-5),
                    ParameterName = "EmailAddress",
                    OldValue = "old.email@example.com",
                    NewValue = "john.doe@example.com",
                    ChangeSource = "User",
                    AccountId = 1
                },
                new AccountHistory
                {
                    Id = 2,
                    ChangeDate = DateTime.UtcNow.AddDays(-3),
                    ParameterName = "PhoneNumber",
                    OldValue = "+0987654321",
                    NewValue = "+1234567890",
                    ChangeSource = "Admin",
                    AccountId = 1
                },
                new AccountHistory
                {
                    Id = 3,
                    ChangeDate = DateTime.UtcNow.AddDays(-10),
                    ParameterName = "Status",
                    OldValue = "Active",
                    NewValue = "Inactive",
                    ChangeSource = "System",
                    AccountId = 2
                }
            );
        }
    }
}
