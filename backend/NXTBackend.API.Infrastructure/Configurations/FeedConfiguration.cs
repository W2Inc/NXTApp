using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class FeedConfiguration : IEntityTypeConfiguration<Feed>
{
    public void Configure(EntityTypeBuilder<Feed> builder)
    {
        // Relationships
        // builder
        //     .HasOne(m => m)
        //     .WithMany(u => u.CreatedFeeds)
        //     .HasForeignKey(m => m.ActorId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}
