using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
{
    public void Configure(EntityTypeBuilder<UserProject> builder)
    {
        builder
            .HasOne(up => up.Project)
            .WithMany(p => p.UserProjects)
            .HasForeignKey(up => up.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(up => up.GitInfo)
            .WithMany(gi => gi.UserProjects)
            .HasForeignKey(up => up.GitInfoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(up => up.Members)
            .WithOne(pm => pm.UserProject)
            .HasForeignKey(up => up.UserProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
