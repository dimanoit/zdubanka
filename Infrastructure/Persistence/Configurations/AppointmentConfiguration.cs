using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments", "public");

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
            .Property(a => a.AppointmentLimitation)
            .HasColumnType("jsonb")
            .IsRequired();

        builder
            .Property(a => a.Location)
            .HasColumnType("jsonb")
            .IsRequired();
    }
}