using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages", "public");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).ValueGeneratedOnAdd();

        builder.Property(a => a.SenderId)
            .IsRequired();

        builder.Property(a => a.Content)
            .IsRequired();

        builder.Property(a => a.SentDate)
            .IsRequired();

        builder.Property(a => a.ChatId)
            .IsRequired();
    }
}