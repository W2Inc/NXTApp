using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(x => x.Login);
        builder.HasIndex(x => x.DisplayName);

        builder
            .HasMany(u => u.CreatedCursus)
            .WithOne(c => c.Creator)
            .HasForeignKey(c => c.CreatorId);

        builder
            .HasMany(u => u.CreatedGoals)
            .WithOne(g => g.Creator)
            .HasForeignKey(g => g.CreatorId);

        builder
            .HasMany(u => u.CreatedProjects)
            .WithOne(p => p.Creator)
            .HasForeignKey(p => p.CreatorId);

        builder
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        builder
            .HasMany(u => u.Rubricer)
            .WithOne(r => r.Reviewer)
            .HasForeignKey(r => r.ReviewerId);
    }
}
