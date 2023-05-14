using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events", "public");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.StartDay).IsRequired();
        builder.Property(a => a.Title).IsRequired();
        builder.Property(a => a.Description).IsRequired();
        builder.Property(a => a.EndDay).IsRequired();
        builder.Property(a => a.OrganizerId).IsRequired();
        builder.Property(a => a.Status).IsRequired();

        builder.Property(a => a.Status)
            .HasConversion(new EnumToStringConverter<EventStatus>())
            .IsRequired();

        builder
            .Property(a => a.EventLimitation)
            .HasColumnType("jsonb")
            .IsRequired();

        builder
            .Property(a => a.Location)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}