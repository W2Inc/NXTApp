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
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /**
         * We avoid using unique constraints in preference to just checking before
         * if the entity exists for example. This is because we want to avoid having 500 errors
         * and actually handle the error in a more graceful way.
         *
         * Only a few models present unique constraints, and those are the ones that are
         * required to be unique / should error in any attempt to create a duplicate.
         */

        UserRelationships(modelBuilder);
        CursusRelationships(modelBuilder);
        LearningGoalRelationships(modelBuilder);
        ProjectRelationships(modelBuilder);
        RubricRelationships(modelBuilder);
        ReviewRelationships(modelBuilder);
        FeedbackRelationships(modelBuilder);
        CommentRelationships(modelBuilder);
        UserDetailsRelationships(modelBuilder);
        MemberRelationships(modelBuilder);
        UserCursusRelationships(modelBuilder);
        UserGoalRelationships(modelBuilder);
        UserProjectRelationships(modelBuilder);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void UserProjectRelationships(ModelBuilder modelBuilder)
    {
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

        // modelBuilder.Entity<UserProject>()
        //     .HasOne(up => up.Rubric)
        //     .WithMany(r => r.UserProjects)
        //     .HasForeignKey(up => up.RubricId)
        //     .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProject>()
            .HasMany(up => up.Members)
            .WithOne(pm => pm.UserProject)
            .HasForeignKey(up => up.UserProjectId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void UserGoalRelationships(ModelBuilder modelBuilder)
    {
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
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void UserCursusRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCursus>()
            .HasOne(uc => uc.User)
            .WithMany(u => u.UserCursi)
            .HasForeignKey(uc => uc.UserId);

        modelBuilder.Entity<UserCursus>()
            .HasOne(uc => uc.Cursus)
            .WithMany(c => c.UserCursi)
            .HasForeignKey(uc => uc.CursusId);

        modelBuilder.Entity<UserCursus>()
            .HasMany(uc => uc.UserGoals)
            .WithOne(ug => ug.UserCursus)
            .HasForeignKey(ug => ug.UserCursusId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void MemberRelationships(ModelBuilder modelBuilder)
    {
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
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void UserDetailsRelationships(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Details>()
        //     .HasOne(d => d.User)
        //     .WithOne(u => u.Details)
        //     .HasForeignKey<Details>(d => d.UserId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void CommentRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Feedback)
            .WithMany(f => f.Comments)
            .HasForeignKey(c => c.FeedbackId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void FeedbackRelationships(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Feedback>()
        //     .HasOne(f => f.Review)
        //     .WithOne(r => r.Feedback)
        //     .HasForeignKey<Feedback>(f => f.ReviewId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void ReviewRelationships(ModelBuilder modelBuilder)
    {
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
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void RubricRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rubric>()
            .HasOne(r => r.Creator)
            .WithMany(u => u.CreatedRubrics)
            .HasForeignKey(r => r.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Rubric>()
            .HasMany(r => r.Reviews)
            .WithOne(rv => rv.Rubric)
            .HasForeignKey(rv => rv.RubricId);

        // modelBuilder.Entity<Rubric>()
        //     .HasMany(r => r.UserProjects)
        //     .WithOne(up => up.Rubric)
        //     .HasForeignKey(up => up.RubricId);

        modelBuilder.Entity<Rubric>()
            .HasOne(r => r.GitInfo)
            .WithMany(gi => gi.Rubrics)
            .HasForeignKey(r => r.GitInfoId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void ProjectRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Creator)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.Rubrics)
            .WithOne(r => r.Project)
            .HasForeignKey(r => r.ProjectId);

        modelBuilder.Entity<Project>()
            .HasMany(p => p.UserProjects)
            .WithOne(up => up.Project)
            .HasForeignKey(up => up.ProjectId);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.GitInfo)
            .WithMany(gi => gi.Projects)
            .HasForeignKey(p => p.GitInfoId);
    }

    private void LearningGoalRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LearningGoal>()
            .HasOne(lg => lg.Creator)
            .WithMany(u => u.CreatedGoals)
            .HasForeignKey(lg => lg.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LearningGoal>()
            .HasMany(lg => lg.Projects)
            .WithMany(p => p.Goals);

        modelBuilder.Entity<LearningGoal>()
            .HasMany(lg => lg.UserGoals)
            .WithOne(ug => ug.Goal)
            .HasForeignKey(ug => ug.GoalId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void CursusRelationships(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cursus>()
            .HasOne(c => c.Creator)
            .WithMany(u => u.CreatedCursus)
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cursus>()
            .HasMany(c => c.UserCursi)
            .WithOne(uc => uc.Cursus)
            .HasForeignKey(uc => uc.CursusId);
    }

    /// <summary>
    /// Relations ships for user model.
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void UserRelationships(ModelBuilder modelBuilder)
    {
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
#nullable restore
}
