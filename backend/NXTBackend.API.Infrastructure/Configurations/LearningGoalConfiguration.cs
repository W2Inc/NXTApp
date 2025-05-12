using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Infrastructure.Configurations;

public class LearningGoalConfiguration : IEntityTypeConfiguration<LearningGoal>
{
    public void Configure(EntityTypeBuilder<LearningGoal> builder)
    {
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Slug);

        builder
            .HasOne(lg => lg.Creator)
            .WithMany(u => u.CreatedGoals)
            .HasForeignKey(lg => lg.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.Collaborators)
            .WithMany(c => c.CollaboratedGoals)
            .UsingEntity("CollaboratorsOnGoals");

        builder
            .HasMany(lg => lg.Projects)
            .WithMany(p => p.Goals);

        builder
            .HasMany(lg => lg.UserGoals)
            .WithOne(ug => ug.Goal)
            .HasForeignKey(ug => ug.GoalId);
    }
}
