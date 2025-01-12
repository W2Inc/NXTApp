using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserGoalConfiguration : IEntityTypeConfiguration<UserGoal>
{
    public void Configure(EntityTypeBuilder<UserGoal> builder)
    {
        builder
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGoals)
            .HasForeignKey(ug => ug.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ug => ug.Goal)
            .WithMany(lg => lg.UserGoals)
            .HasForeignKey(ug => ug.GoalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(ug => ug.UserCursus)
            .WithMany(uc => uc.UserGoals)
            .HasForeignKey(ug => ug.UserCursusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
