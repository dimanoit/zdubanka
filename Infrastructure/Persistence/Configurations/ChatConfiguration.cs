using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable("Chats", "public");
        builder.HasKey(a => a.AppointmentId);

        builder.Property(a => a.CreationDate).IsRequired();

        builder.HasMany(a => a.Messages)
            .WithOne(ap => ap.Chat)
            .HasForeignKey(ap => ap.ChatId)
            .IsRequired();

        builder.HasOne(c => c.Appointment)
            .WithOne(a => a.Chat)
            .HasForeignKey<Chat>(a => a.AppointmentId)
            .IsRequired();
    }
}