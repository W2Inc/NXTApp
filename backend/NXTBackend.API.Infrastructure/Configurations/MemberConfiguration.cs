using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder
            .HasOne(m => m.User)
            .WithMany(u => u.ProjectMember)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(m => m.UserProject)
            .WithMany(up => up.Members)
            .HasForeignKey(m => m.UserProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
