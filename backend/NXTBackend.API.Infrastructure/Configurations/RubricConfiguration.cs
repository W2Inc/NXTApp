using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Infrastructure.Configurations;

public class RubricConfiguration : IEntityTypeConfiguration<Rubric>
{
    public void Configure(EntityTypeBuilder<Rubric> builder)
    {
        builder.HasIndex(x => x.Name);

        builder
            .HasOne(r => r.Creator)
            .WithMany(u => u.CreatedRubrics)
            .HasForeignKey(r => r.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(r => r.Reviews)
            .WithOne(rv => rv.Rubric)
            .HasForeignKey(rv => rv.RubricId);

        // builder
        //     .HasMany(r => r.UserProjects)
        //     .WithOne(up => up.Rubric)
        //     .HasForeignKey(up => up.RubricId);

        builder
            .HasOne(r => r.GitInfo)
            .WithMany(gi => gi.Rubrics)
            .HasForeignKey(r => r.GitInfoId);
    }
}
