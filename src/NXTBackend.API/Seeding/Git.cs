// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Bogus;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;

namespace NXTBackend.API.Seeding;

// ============================================================================

/// <summary>
/// Seeder for the git entity.
/// </summary>
/// <param name="_databaseContext">The database context</param>
public class GitSeeder : BaseSeeder<Git>
{
    public GitSeeder(DatabaseContext _databaseContext) : base(_databaseContext) { }

    public override async Task SeedAsync()
    {
        await _databaseContext.AddRangeAsync(GenerateEntities(10));
        await _databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public static IEnumerable<Git> GenerateEntities(int count) => _generator.Generate(count);

    private static readonly Faker<Git> _generator = new Faker<Git>()
        .Rules((f, g) =>
        {
            g.GitBranch = "master";
            g.GitCommit = f.Hacker.Random.String(7, 'a', 'z');
            g.GitUrl = $"{f.Internet.Url()}/git";
        });
}
