using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RQL2Expression.Core.Models;

namespace RQL2Expression.Data.Context.Configurations
{
    public class AccountHistoryConfiguration : IEntityTypeConfiguration<AccountHistory>
    {
        public void Configure(EntityTypeBuilder<AccountHistory> builder)
        {
            builder.ToTable("account_history");

            builder.HasKey(ah => ah.Id);

            builder.Property(ah => ah.Id)
                .ValueGeneratedOnAdd();

            builder.Property(ah => ah.ChangeDate)
                .HasColumnName("change_date")
                .IsRequired();

            builder.Property(ah => ah.ParameterName)
                .HasColumnName("parameter_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(ah => ah.NewValue)
                .HasColumnName("new_value")
                .HasMaxLength(200);

            builder.Property(ah => ah.OldValue)
                .HasColumnName("old_value")
                .HasMaxLength(200);

            builder.Property(ah => ah.ChangeSource)
                .HasColumnName("change_source")
                .HasMaxLength(100);

            builder.Property(ah => ah.AccountId)
                .HasColumnName("account_id")
                .IsRequired();

            builder.HasOne(ah => ah.Account)
                .WithMany(a => a.History)
                .HasForeignKey(ah => ah.AccountId);
        }
    }
}
