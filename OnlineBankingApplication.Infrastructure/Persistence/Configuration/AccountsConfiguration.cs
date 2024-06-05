using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineBankingApplication.Domain.Entities;

namespace OnlineBankingApplication.Infrastructure.Persistence.Configuration;

public class AccountsConfiguration : IEntityTypeConfiguration<Accounts>
{
    public void Configure(EntityTypeBuilder<Accounts> builder)
    {
        builder.Property(t => t.AccountBalance).HasPrecision(18, 4);
        builder.Property(t => t.AccountHolderName).IsRequired().HasMaxLength(50);

    }
}
