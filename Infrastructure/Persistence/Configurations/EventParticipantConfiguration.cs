using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class EventParticipantConfiguration : IEntityTypeConfiguration<EventParticipant>
{
    public void Configure(EntityTypeBuilder<EventParticipant> builder)
    {
        builder.ToTable("EventParticipants");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .IsRequired();
        builder.Property(x => x.EventId)
            .IsRequired();

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .IsRequired();

        builder
            .HasIndex(x => new { x.UserId, x.EventId })
            .IsUnique();

        builder.HasOne(ap => ap.Account)
            .WithMany(ac => ac.EventParticipations)
            .HasForeignKey(x => x.UserId);

        builder.HasOne(ap => ap.Event)
            .WithMany(ac => ac.EventParticipants)
            .HasForeignKey(x => x.EventId);
    }
}
