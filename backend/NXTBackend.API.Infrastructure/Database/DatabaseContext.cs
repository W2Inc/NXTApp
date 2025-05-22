using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Spotlight;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Infrastructure.Configurations;

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
        // modelBuilder.HasPostgresEnum<TaskState>();
        // modelBuilder.HasPostgresEnum<CursusKind>();
        // modelBuilder.HasPostgresEnum<MemberInviteState>();
        // modelBuilder.HasPostgresEnum<NotificationState>();
        // modelBuilder.HasPostgresEnum<ReviewKind>();
        // modelBuilder.HasPostgresEnum<ReviewState>();

        // Entities
        new ProjectConfiguration().Configure(modelBuilder.Entity<Project>());
        new LearningGoalConfiguration().Configure(modelBuilder.Entity<LearningGoal>());
        new CursusConfiguration().Configure(modelBuilder.Entity<Cursus>());

        // Evalution
        new FeedbackConfiguration().Configure(modelBuilder.Entity<Feedback>());
        new RubricConfiguration().Configure(modelBuilder.Entity<Rubric>());
        new CommentConfiguration().Configure(modelBuilder.Entity<Comment>());

        // User
        new UserConfiguration().Configure(modelBuilder.Entity<User>());
        new UserCursusConfiguration().Configure(modelBuilder.Entity<UserCursus>());
        new UserGoalConfiguration().Configure(modelBuilder.Entity<UserGoal>());
        new UserProjectConfiguration().Configure(modelBuilder.Entity<UserProject>());

        // Other
        new MemberConfiguration().Configure(modelBuilder.Entity<Member>());
        new NotificationConfiguration().Configure(modelBuilder.Entity<Notification>());
        new UserFeedConfiguration().Configure(modelBuilder.Entity<UserFeed>());
        new NotificationConfiguration().Configure(modelBuilder.Entity<Notification>());
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
    public DbSet<Feed> Feeds { get; set; }
#nullable restore
}
