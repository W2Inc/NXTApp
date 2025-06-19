using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Evaluation.v2;
using NXTBackend.API.Domain.Entities.Spotlight;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Domain.Joins;

namespace NXTBackend.API.Infrastructure.Database;


public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

#nullable disable
    public DbSet<Feature> Features { get; set; }
    public DbSet<Details> Details { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<SpotlightEvent> SpotlightEvents { get; set; }
    public DbSet<SpotlightEventAction> SpotlightEventsActions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Cursus> Cursi { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<LearningGoal> LearningGoals { get; set; }
    public DbSet<UserCursus> UserCursi { get; set; }
    public DbSet<UserGoal> UserGoals { get; set; }
    public DbSet<UserProject> UserProject { get; set; }
    public DbSet<Rubric> Rubric { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Git> GitInfo { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<UserFeed> UserFeeds { get; set; }

    // Joins
    public DbSet<GoalProject> GoalProject { get; set; }
    public DbSet<GoalCollaborator> GoalCollaborator { get; set; }
    public DbSet<CursusCollaborator> CursusCollaborator { get; set; }
#nullable restore
}
