using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Slug);

        builder
            .HasOne(p => p.Creator)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(p => p.Rubrics)
            .WithOne(r => r.Project)
            .HasForeignKey(r => r.ProjectId);

        builder
            .HasMany(p => p.UserProjects)
            .WithOne(up => up.Project)
            .HasForeignKey(up => up.ProjectId);

        builder
            .HasOne(p => p.GitInfo)
            .WithMany(gi => gi.Projects)
            .HasForeignKey(p => p.GitInfoId);
    }
}
