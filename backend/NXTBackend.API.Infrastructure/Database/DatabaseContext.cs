using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Evaluation;
using NXTBackend.API.Domain.Entities.Notification;
using NXTBackend.API.Domain.Entities.Users;

namespace NXTBackend.API.Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        //optionsBuilder.AddInterceptors(new SavingChangesInterceptor(TimeProvider.System));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //UserSeeder.Generate(5);
        //modelBuilder.Entity<User>().HasData(UserSeeder.FakeUsers);
        //modelBuilder.Entity<Details>().HasData(UserSeeder.FakeDetails);

        /**
         * We avoid using unique constraints in preference to just checking before
         * if the entity exists for example. This is because we want to avoid having 500 errors
         * and actually handle the error in a more graceful way.
         *
         * Only a few models present unique constraints, and those are the ones that are
         * required to be unique / should error in any attempt to create a duplicate.
         */

        // ============================================================================
        //= Cursus relationships =//
        // ============================================================================

        modelBuilder.Entity<Cursus>()
            .HasOne(c => c.Creator)
            .WithMany(u => u.CreatedCursus)
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cursus>()
            .HasMany(c => c.UserCursi)
            .WithOne(uc => uc.Cursus)
            .HasForeignKey(uc => uc.CursusId);

        // ============================================================================
        //= LearningGoal relationships =//
        // ============================================================================

        modelBuilder.Entity<LearningGoal>()
            .HasOne(lg => lg.Creator)
            .WithMany(u => u.CreatedGoals)
            .HasForeignKey(lg => lg.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // A LearningGoal can have multiple Projects
        modelBuilder.Entity<LearningGoal>()
            .HasMany(lg => lg.Projects)
            .WithMany(p => p.Goals);

        // A LearningGoal can have multiple UserGoals
        modelBuilder.Entity<LearningGoal>()
            .HasMany(lg => lg.UserGoals)
            .WithOne(ug => ug.Goal)
            .HasForeignKey(ug => ug.GoalId);

        // ============================================================================
        //= Project relationships =//
        // ============================================================================

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Creator)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // A project can have multiple rubrics
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Rubrics)
            .WithOne(r => r.Project)
            .HasForeignKey(r => r.ProjectId);

        // A project can have multiple UserProjects
        modelBuilder.Entity<Project>()
            .HasMany(p => p.UserProjects)
            .WithOne(up => up.Project)
            .HasForeignKey(up => up.ProjectId);

        // A project references a single GitInfo
        modelBuilder.Entity<Project>()
            .HasOne(p => p.GitInfo)
            .WithMany(gi => gi.Projects)
            .HasForeignKey(p => p.GitInfoId);

        // ============================================================================
        //= Rubric relationships =//
        // ============================================================================

        modelBuilder.Entity<Rubric>()
            .HasOne(r => r.Creator)
            .WithMany(u => u.CreatedRubrics)
            .HasForeignKey(r => r.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        // A rubric can have multiple reviews
        modelBuilder.Entity<Rubric>()
            .HasMany(r => r.Reviews)
            .WithOne(rv => rv.Rubric)
            .HasForeignKey(rv => rv.RubricId);

        // A rubric can have multiple UserProjects
        modelBuilder.Entity<Rubric>()
            .HasMany(r => r.UserProjects)
            .WithOne(up => up.Rubric)
            .HasForeignKey(up => up.RubricId);

        // Git Info
        modelBuilder.Entity<Rubric>()
            .HasOne(r => r.GitInfo)
            .WithMany(gi => gi.Rubrics)
            .HasForeignKey(r => r.GitInfoId);

        // ============================================================================
        //= Review relationships =//
        // ============================================================================

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Rubric)
            .WithMany(r => r.Reviews)
            .HasForeignKey(r => r.RubricId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Reviewer)
            .WithMany(u => u.Rubricer)
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================================
        //= Feedback relationships =//
        // ============================================================================

        // ID towards review must be unique
        //modelBuilder.Entity<Feedback>()
        //    .HasIndex(r => r.ReviewId)
        //    .IsUnique();

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Review)
            .WithOne(r => r.Feedback)
            .HasForeignKey<Feedback>(f => f.ReviewId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================================
        //= Comment relationships =//
        // ============================================================================

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // A comment to a feedback
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Feedback)
            .WithMany(f => f.Comments)
            .HasForeignKey(c => c.FeedbackId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================================
        //= User Details relationships =//
        // ============================================================================

        modelBuilder.Entity<Details>()
            .HasOne(d => d.User)
            .WithOne(u => u.Details)
            .HasForeignKey<Details>(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================================
        //= User relationships =//
        // ============================================================================

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedCursus)
            .WithOne(c => c.Creator)
            .HasForeignKey(c => c.CreatorId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedGoals)
            .WithOne(g => g.Creator)
            .HasForeignKey(g => g.CreatorId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedProjects)
            .WithOne(p => p.Creator)
            .HasForeignKey(p => p.CreatorId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Rubricer)
            .WithOne(r => r.Reviewer)
            .HasForeignKey(r => r.ReviewerId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(r => r.UserId);

        // ============================================================================
        //= Project Member relationships =//
        // ============================================================================

        modelBuilder.Entity<Member>()
            .HasOne(m => m.User)
            .WithMany(u => u.ProjectMember)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.UserProject)
            .WithMany(up => up.Members)
            .HasForeignKey(m => m.UserProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Member>()
            .HasOne(m => m.UserGoal)
            .WithMany(ug => ug.Members)
            .HasForeignKey(m => m.UserProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================================
        //= UserCursus relationships =//
        // ============================================================================

        // A user can have multiple UserCursi
        modelBuilder.Entity<UserCursus>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCursi)
            .HasForeignKey(uc => uc.UserId);

        // A Cursus can have multiple UserCursi
        modelBuilder.Entity<UserCursus>()
            .HasOne(uc => uc.Cursus)
            .WithMany(c => c.UserCursi)
            .HasForeignKey(uc => uc.CursusId);

        // A User goal references a UserCursus
        modelBuilder.Entity<UserCursus>()
            .HasMany(uc => uc.UserGoals)
            .WithOne(ug => ug.UserCursus)
            .HasForeignKey(ug => ug.UserCursusId);

        // ============================================================================
        //= User Goal relationships =//
        // ============================================================================

        modelBuilder.Entity<UserGoal>()
            .HasOne(ug => ug.User)
            .WithMany(u => u.UserGoals)
            .HasForeignKey(ug => ug.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserGoal>()
            .HasOne(ug => ug.Goal)
            .WithMany(lg => lg.UserGoals)
            .HasForeignKey(ug => ug.GoalId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserGoal>()
            .HasOne(ug => ug.UserCursus)
            .WithMany(uc => uc.UserGoals)
            .HasForeignKey(ug => ug.UserCursusId)
            .OnDelete(DeleteBehavior.Restrict);

        // ============================================================================
        //= User Project relationships =//
        // ============================================================================

        modelBuilder.Entity<UserProject>()
            .HasOne(up => up.Project)
            .WithMany(p => p.UserProjects)
            .HasForeignKey(up => up.ProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProject>()
            .HasOne(up => up.GitInfo)
            .WithMany(gi => gi.UserProjects)
            .HasForeignKey(up => up.GitInfoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProject>()
            .HasOne(up => up.Rubric)
            .WithMany(r => r.UserProjects)
            .HasForeignKey(up => up.RubricId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProject>()
            .HasMany(up => up.Members)
            .WithOne(pm => pm.UserProject)
            .HasForeignKey(up => up.UserProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }

#nullable disable
    public DbSet<Feature> Features { get; set; }
    public DbSet<Details> Details { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationAction> NotificationActions { get; set; }
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
#nullable restore
}
