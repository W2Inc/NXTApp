using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class UserCursusConfiguration : IEntityTypeConfiguration<UserCursus>
{
    public void Configure(EntityTypeBuilder<UserCursus> builder)
    {
        builder
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCursi)
            .HasForeignKey(uc => uc.UserId);

        builder
            .HasOne(uc => uc.Cursus)
            .WithMany(c => c.UserCursi)
            .HasForeignKey(uc => uc.CursusId);

        builder
            .HasMany(uc => uc.UserGoals)
            .WithOne(ug => ug.UserCursus)
            .HasForeignKey(ug => ug.UserCursusId);
    }
}
