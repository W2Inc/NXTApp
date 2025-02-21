using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
{
    public void Configure(EntityTypeBuilder<UserNotification> builder)
    {
        // Indexes
        builder.HasIndex(un => un.UserId);
        builder.HasIndex(un => un.Status);
        builder.HasIndex(un => un.CreatedAt);

        // Properties
        builder.Property(un => un.Status)
            .IsRequired();

        // Unique constraint
        builder.HasIndex(un => new { un.UserId, un.NotificationId })
            .IsUnique();

        // Relationships
        builder.HasOne(un => un.User)
            .WithMany(u => u.UserNotifications)
            .HasForeignKey(un => un.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(un => un.Notification)
            .WithMany(n => n.UserNotifications)
            .HasForeignKey(un => un.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
