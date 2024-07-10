// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Bogus;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Infrastructure.Database;

// ============================================================================

namespace NXTBackend.API.Seeding;

/// </inheritdoc>
public class UserSeeder : BaseSeeder<User>
{
    public UserSeeder(DatabaseContext _databaseContext) : base(_databaseContext) { }

    /// <inheritdoc/>
    public override async Task SeedAsync()
    {
        const int C_LENGTH = 10;
        var users = GenerateEntities(C_LENGTH);
        var details = DetailsSeeder.GenerateEntities(C_LENGTH);
        for (int i = 0; i < C_LENGTH; i++)
        {
            var user = users.ElementAt(i) ?? throw new NullReferenceException("Shouldn't happen!");
            var detail = details.ElementAt(i) ?? throw new NullReferenceException("Shouldn't happen!");

            user.DetailsId = detail.Id;
            detail.UserId = user.Id;
        }

        await _databaseContext.Users.AddRangeAsync(users);
        await _databaseContext.Details.AddRangeAsync(details);
        await _databaseContext.SaveChangesAsync();
    }

    public static IEnumerable<User> GenerateEntities(int count) => _generator.Generate(count);


    private static readonly Faker<User> _generator = new Faker<User>()
        .Rules((f, u) =>
        {
            u.Login = f.Internet.UserName();
            u.DisplayName = f.Name.FirstName();
            u.AvatarUrl = f.Image.PicsumUrl();
        });
}

