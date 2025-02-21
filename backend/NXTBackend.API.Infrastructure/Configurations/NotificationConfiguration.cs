using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // Indexes
        builder.HasIndex(n => n.Kind);
        builder.HasIndex(n => n.CreatedAt);

        // Properties
        builder.Property(n => n.Message)
            .IsRequired();

        builder.Property(n => n.Kind)
            .IsRequired();

        // Relationships
        builder.HasMany(n => n.UserNotifications)
            .WithOne(un => un.Notification)
            .HasForeignKey(un => un.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
