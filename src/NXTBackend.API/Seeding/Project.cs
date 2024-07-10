// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Bogus;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;
using Serilog;

namespace NXTBackend.API.Seeding;

// ============================================================================

/// <summary>
/// Seeder for the user entity.
/// </summary>
/// <param name="_databaseContext">The database context</param>
public class ProjectSeeder : BaseSeeder<Project>
{
    public ProjectSeeder(DatabaseContext _databaseContext) : base(_databaseContext) { }

    /// <inheritdoc/>
    public override async Task SeedAsync()
    {

        var user = UserSeeder.GenerateEntities(1).First();
        var details = DetailsSeeder.GenerateEntities(1).First();

        user.DetailsId = details.Id;
        details.UserId = user.Id;


        var gitInfo = GitSeeder.GenerateEntities(1).First();

        var project = GenerateEntities(1).First();
        var newproject = new Project
        {
            Name = project.Name,
            Description = project.Description,
            Enabled = project.Enabled,
            Public = project.Public,
            MaxMembers = project.MaxMembers,
            Markdown = project.Markdown,

            CreatorId = user.Id,
            GitInfoId = gitInfo.Id
        };

        _databaseContext.AddRange(user, details, gitInfo, newproject);
        await _databaseContext.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public static new IEnumerable<Project> GenerateEntities(int count) => _generator.Generate(count);

    private static readonly Faker<Project> _generator = new Faker<Project>()
        .Rules((f, u) =>
        {
            u.Name = f.Lorem.Word();
            u.Description = f.Lorem.Sentence();
            u.Enabled = true;
            u.Public = true;
            u.MaxMembers = 3;
            u.Markdown = f.Lorem.Paragraph();
        });
}

