using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Infrastructure.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder
            .HasOne(r => r.Rubric)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.RubricId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.Reviewer)
            .WithMany(u => u.Rubricer)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
