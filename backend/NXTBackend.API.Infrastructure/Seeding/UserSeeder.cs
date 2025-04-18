using Bogus;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Seeding;

public static class UserSeeder
{
    public static List<User> Users = new();
    public static List<Details> Details = new();

    public static void Init(int count)
    {
        var detailsFaker = new Faker<Details>()
            .RuleFor(d => d.Email, f => f.Internet.Email())
            .RuleFor(d => d.Markdown, f => f.Lorem.Paragraph())
            .RuleFor(d => d.FirstName, f => f.Name.FirstName())
            .RuleFor(d => d.LastName, f => f.Name.LastName())
            .RuleFor(d => d.GithubUrl, f => f.Internet.Url())
            .RuleFor(d => d.RedditUrl, f => f.Internet.Url())
            .RuleFor(d => d.WebsiteUrl, f => f.Internet.Url())
            .RuleFor(d => d.LinkedinUrl, f => f.Internet.Url());

        var userFaker = new Faker<User>()
           .RuleFor(u => u.Login, f => f.Internet.UserName())
           .RuleFor(u => u.DisplayName, f => f.Name.FirstName())
           .RuleFor(u => u.AvatarUrl, f => f.Image.PicsumUrl());

        for (int i = 0; i < count; i++)
        {
            // Generate a new user
            var user = userFaker.Generate();
            Users.Add(user);

            // Generate a new details entity and link it to the user
            var detail = detailsFaker.Generate();
            detail.UserId = user.Id;
            user.DetailsId = detail.Id;
            Details.Add(detail);
        }
    }
}
