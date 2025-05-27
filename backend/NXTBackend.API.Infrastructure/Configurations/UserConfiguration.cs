using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // ✅ KEEP: Essential for authentication
        builder.HasIndex(x => x.Login)
            .IsUnique()
            .HasDatabaseName("IX_user_login_unique");

        // ✅ KEEP: This composite covers both login lookups AND search functionality
        builder.HasIndex(x => new { x.Login, x.DisplayName })
            .HasDatabaseName("IX_user_login_display")
            .IncludeProperties(x => new { x.AvatarUrl, x.CreatedAt, x.UpdatedAt });

        // ✅ KEEP: Essential for joins (if you frequently load user details)
        builder.HasIndex(x => x.DetailsId)
            .HasDatabaseName("IX_user_details_id")
            .HasFilter("details_id IS NOT NULL");

        // ✅ KEEP: One covering index for all temporal queries
        builder.HasIndex(x => x.CreatedAt)
            .HasDatabaseName("IX_user_temporal_covering")
            .IsDescending()
            .IncludeProperties(x => new { x.Login, x.DisplayName, x.AvatarUrl, x.UpdatedAt });

        // Relationships remain the same...
        ConfigureRelationships(builder);
    }

    private static void ConfigureRelationships(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.CreatedCursus).WithOne(c => c.Creator).HasForeignKey(c => c.CreatorId);
        builder.HasMany(u => u.CreatedGoals).WithOne(g => g.Creator).HasForeignKey(g => g.CreatorId);
        builder.HasMany(u => u.CreatedProjects).WithOne(p => p.Creator).HasForeignKey(p => p.CreatorId);
        builder.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId);
        builder.HasMany(u => u.Rubricer).WithOne(r => r.Reviewer).HasForeignKey(r => r.ReviewerId);
    }
}