using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AccountTokensConfiguration: IEntityTypeConfiguration<AccountToken>
{
    public void Configure(EntityTypeBuilder<AccountToken> builder)
    {
        builder.ToTable("AccountTokens", "public");
        builder.HasKey(a => a.Token);

        builder.HasOne(at => at.Account)
            .WithOne(ac => ac.Token)
            .HasForeignKey<AccountToken>(at => at.AccountId);
    }
}

