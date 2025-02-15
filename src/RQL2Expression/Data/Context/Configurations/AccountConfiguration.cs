using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RQL2Expression.Core.Models;

namespace RQL2Expression.Data.Context.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");

            builder.HasKey(c => c.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(m => m.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            builder.Property(m => m.Status)
                .HasColumnType("VARCHAR(10)")
                .HasDefaultValue(AccountStatus.Active)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(m => m.LastLoginDate)
                .HasColumnName("login_last_date")
                .IsRequired(false);

            builder.Property(m => m.LastName)
                .HasColumnName("last_name")
                .IsRequired();

            builder.Property(m => m.FirstName)
                .HasColumnName("first_name")
                .IsRequired();

            builder.Property(m => m.MiddleName)
                .HasColumnName("middle_name")
                .IsRequired(false);

            builder.Property(m => m.DateOfBirth)
                .HasColumnName("date_birth")
                .IsRequired();

            builder.Property(m => m.EmailAddress)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(m => m.EmailVerified)
                .HasColumnName("email_verified")
                .HasDefaultValue(false)
                .IsRequired();


            builder.Property(m => m.PhoneNumber)
                .HasColumnName("phone")
                .IsRequired();

            builder.Property(m => m.PhoneVerified)
                .HasColumnName("phone_verified")
                .IsRequired();

            builder
                .HasMany(a => a.History)
                .WithOne(h => h.Account)
                .HasForeignKey(h => h.AccountId);
        }
    }
}
