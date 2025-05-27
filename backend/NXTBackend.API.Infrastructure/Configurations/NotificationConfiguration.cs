using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        // ✅ KEEP: Most important composite index for unread notifications
        builder.HasIndex(n => new { n.NotifiableId, n.ReadAt })
            .HasDatabaseName("IX_notifications_notifiable_read")
            .IncludeProperties(n => new { n.CreatedAt, n.Descriptor, n.Type });

        // ✅ KEEP: Essential for background processing
        builder.HasIndex(n => n.State)
            .HasDatabaseName("IX_notifications_state")
            .HasFilter("state = 0");

        // ✅ KEEP: Only if you frequently query notifications by resource
        // Consider removing if this query pattern is rare
        builder.HasIndex(n => new { n.ResourceId, n.CreatedAt })
            .HasDatabaseName("IX_notifications_resource_created")
            .IsDescending(false, true);
    }
}