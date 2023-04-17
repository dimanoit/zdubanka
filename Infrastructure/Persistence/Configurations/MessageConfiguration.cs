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

        builder.Property(m => m.SenderId).IsRequired();
        builder.Property(m => m.Content).IsRequired();
        builder.Property(m => m.SentDate).IsRequired();

        builder.HasKey(a => a.Id);
    }
}