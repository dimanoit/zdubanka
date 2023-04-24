using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Accounts", "public");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.RefreshToken).IsRequired(false);
        builder.Property(a => a.RefreshTokenExpiryTime).IsRequired(false);
        builder.Property(a => a.Id).IsRequired();
        builder.Property(a => a.Email).IsRequired();
        builder.Property(a => a.FullName).IsRequired();
        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email");

        builder.Property(a => a.AuthMethod)
            .HasConversion(new EnumToStringConverter<AuthMethod>())
            .IsRequired();

        builder.Property(a => a.Gender)
            .HasConversion(new EnumToStringConverter<Gender>());

        builder.Property(a => a.RelationshipStatus)
            .HasConversion(new EnumToStringConverter<RelationshipStatus>());

        builder
            .Property(a => a.UserLanguages)
            .HasColumnType("jsonb");

        builder.HasMany(a => a.Appointments)
            .WithOne(ap => ap.Organizer)
            .HasForeignKey(ap => ap.OrganizerId)
            .IsRequired();
    }
}