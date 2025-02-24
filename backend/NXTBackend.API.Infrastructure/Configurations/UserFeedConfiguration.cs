using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserFeedConfiguration : IEntityTypeConfiguration<UserFeed>
{
    public void Configure(EntityTypeBuilder<UserFeed> builder)
    {
        // Relationships
        builder.HasOne(x => x.User)
            .WithOne(u => u.UserFeed)
            .HasForeignKey<UserFeed>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
