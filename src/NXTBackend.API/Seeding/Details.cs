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
public class DetailsSeeder : BaseSeeder<Details>
{
    public DetailsSeeder(DatabaseContext _databaseContext) : base(_databaseContext) { }

    /// <summary>
    /// Seed the database with fake data.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public override async Task SeedAsync() => throw new NotSupportedException("Details cannot be seeded. Instead they get inserted during user seeding");

    /// <inheritdoc/>
    public static new IEnumerable<Details> GenerateEntities(int count) => _generator.Generate(count);

    private static readonly Faker<Details> _generator = new Faker<Details>()
        .Rules((f, g) =>
        {
            g.Email = f.Internet.Email();
            g.Bio = f.Lorem.Paragraphs();
            g.FirstName = f.Name.FirstName();
            g.LastName = f.Name.LastName();
            g.GithubUrl = f.Internet.Url();
            g.TwitterUrl = f.Internet.Url();
            g.WebsiteUrl = f.Internet.Url();
            g.LinkedinUrl = f.Internet.Url();
        });
}
