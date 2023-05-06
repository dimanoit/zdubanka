using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AppointmentParticipantConfiguration : IEntityTypeConfiguration<AppointmentParticipant>
{
    public void Configure(EntityTypeBuilder<AppointmentParticipant> builder)
    {
        builder.ToTable("AppointmentParticipants");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .IsRequired();
        builder.Property(x => x.AppointmentId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder
            .HasIndex(x => new { x.UserId, x.AppointmentId })
            .IsUnique();

        builder.HasOne(ap => ap.Account)
            .WithMany(ac => ac.AppointmentParticipations)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(ap => ap.Appointment)
            .WithMany(ac => ac.AppointmentParticipants)
            .HasForeignKey(x => x.AppointmentId);
    }
}
