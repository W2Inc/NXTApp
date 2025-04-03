using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Infrastructure.Configurations;

public class CursusConfiguration : IEntityTypeConfiguration<Cursus>
{
    public void Configure(EntityTypeBuilder<Cursus> builder)
    {
        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.Slug);

        builder
            .HasMany(c => c.Collaborators)
            .WithMany(c => c.CollaboratesOnCursi)
            .UsingEntity("CollaboratosToCursi");

        builder
            .HasOne(c => c.Creator)
                .WithMany(u => u.CreatedCursus)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(c => c.UserCursi)
                .WithOne(uc => uc.Cursus)
                .HasForeignKey(uc => uc.CursusId);
    }
}
