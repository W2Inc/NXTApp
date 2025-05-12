using Bogus;
using Microsoft.Extensions.DependencyInjection;
using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Domain.Entities;

namespace NXTBackend.API.Tests.Services;

public class GoalServiceTest : BaseServiceTest
{
    private readonly IGoalService _goalService;
    private readonly IUserService _userService;

    public GoalServiceTest() : base() // Optional: Use a fixed seed for deterministic test data
    {
        _goalService = ServiceProvider.GetRequiredService<IGoalService>();
        _userService = ServiceProvider.GetRequiredService<IUserService>();
    }

    [Fact]
    public async Task CreateGoal_ValidGoal_SavesGoal()
    {
        // Arrange
        var goal = new LearningGoal
        {
            Name = "Test Goal",
            Description = "A test goal for unit testing"
        };

        // Act
        var result = await _goalService.CreateAsync(goal);

        // Assert
        Assert.NotNull(result);

        // Verify it's in the database
        var savedGoal = await DbContext.LearningGoals.FindAsync(result.Id);
        Assert.NotNull(savedGoal);
        Assert.Equal("Test Goal", savedGoal.Name);
    }

    [Fact]
    public async Task FindById_ExistingGoal_ReturnsGoal()
    {
        // Arrange
        var goal = new LearningGoal
        {
            Name = Faker.Name.JobType(),
            Description = Faker.Lorem.Paragraph()
        };

        DbContext.LearningGoals.Add(goal);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _goalService.FindByIdAsync(goal.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(goal.Id, result.Id);
        Assert.Equal(goal.Name, result.Name);
    }

    [Fact]
    public async Task FindById_NonExistentGoal_ReturnsNull()
    {
        var result = await _goalService.FindByIdAsync(Guid.NewGuid());
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteGoal_ExistingGoal_RemovesFromDatabase()
    {
        // Arrange
        var goal = new LearningGoal
        {
            Name = Faker.Commerce.ProductName(),
            Description = Faker.Lorem.Sentence()
        };

        DbContext.LearningGoals.Add(goal);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _goalService.DeleteAsync(goal);

        // Assert
        Assert.NotNull(result);

        // Verify it's removed from the database
        var deletedGoal = await DbContext.LearningGoals.FindAsync(goal.Id);
        Assert.Null(deletedGoal);
    }

    [Fact]
    public async Task CreateGoal_WithRandomData_SavesCorrectly()
    {
        // Create a goal with random data using Faker
        var goal = new LearningGoal
        {
            Name = Faker.Hacker.Phrase(),
            Description = Faker.Lorem.Paragraph(2)
        };

        var result = await _goalService.CreateAsync(goal);

        Assert.NotNull(result);
        Assert.Equal(goal.Name, result.Name);
        Assert.Equal(goal.Description, result.Description);
    }

    [Fact]
    public async Task AddCollaborator_ToNewGoal()
    {
        var user = await _userService.CreateAsync(new()
        {
            Login = Faker.Internet.UserName(),
        });

        Assert.NotNull(user);
        Assert.NotEqual(Guid.Empty, user.Id);
        var goal = new LearningGoal
        {
            Name = Faker.Hacker.Phrase(),
            Description = Faker.Lorem.Paragraph(2),
            CreatorId = user.Id
        };

        var createdGoal = await _goalService.CreateAsync(goal);
        Assert.NotNull(createdGoal);

        await _goalService.AddCollaborator(createdGoal.Id, user.Id);
        var updatedGoal = await _goalService.FindByIdAsync(createdGoal.Id);
        Assert.NotNull(updatedGoal);

        var collaboratorResult = await _goalService.IsCollaborator(updatedGoal.Id, user.Id);
        Assert.NotNull(collaboratorResult.Item1); // Learning goal exists
        Assert.NotNull(collaboratorResult.Item2); // User exists as collaborator
    }
}
