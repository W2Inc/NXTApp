using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Infrastructure.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        // modelBuilder.Entity<Feedback>()
        //     .HasOne(f => f.Review)
        //     .WithOne(r => r.Feedback)
        //     .HasForeignKey<Feedback>(f => f.ReviewId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}
