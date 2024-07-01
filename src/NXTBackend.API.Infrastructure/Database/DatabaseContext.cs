using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Domain.Entities.Event;
using NXTBackend.API.Domain.Entities.Review;
using NXTBackend.API.Domain.Entities.User;
using NXTBackend.API.Domain.Entities.User.Project;

namespace NXTBackend.API.Infrastructure.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.AddInterceptors(new SavingChangesInterceptor(TimeProvider.System));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cursus>()
            .HasOne(c => c.Creator)
            .WithMany(u => u.CreatedCursus)
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cursus>()
            .HasMany(c => c.Vertices)
            .WithOne(cv => cv.Cursus)
            .HasForeignKey(cv => cv.CursusId);

        //modelBuilder.Entity<Cursus>()
        //    .HasMany(c => c.UserCursi)
        //    .WithOne(uc => uc.Cursus)
        //    .HasForeignKey(uc => uc.CursusId);

        //// CursusVertex relationships
        //modelBuilder.Entity<CursusVertex>()
        //    .HasOne(cv => cv.Cursus)
        //    .WithMany(c => c.Vertices)
        //    .HasForeignKey(cv => cv.CursusId);

        //modelBuilder.Entity<CursusVertex>()
        //    .HasOne(cv => cv.Parent)
        //    .WithMany(p => p.Children)
        //    .HasForeignKey(cv => cv.ParentId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<CursusVertex>()
        //    .HasMany(cv => cv.Goals)
        //    .WithMany(g => g.GoalVertices);

        //modelBuilder.Entity<CursusVertex>()
        //    .HasMany(cv => cv.UserGoals)
        //    .WithOne(ug => ug.Vertex)
        //    .HasForeignKey(ug => ug.VertexId);

        //// UserCursus relationships
        //modelBuilder.Entity<UserCursus>()
        //    .HasOne(uc => uc.User)
        //    .WithMany(u => u.Cursi)
        //    .HasForeignKey(uc => uc.UserId);

        //modelBuilder.Entity<UserCursus>()
        //    .HasOne(uc => uc.Cursus)
        //    .WithMany(c => c.UserCursi)
        //    .HasForeignKey(uc => uc.CursusId);

        //modelBuilder.Entity<UserCursus>()
        //    .HasMany(uc => uc.UserGoals)
        //    .WithOne(ug => ug.UserCursus)
        //    .HasForeignKey(ug => ug.UserCursusId);

        //// LearningGoal relationships
        //modelBuilder.Entity<LearningGoal>()
        //    .HasOne(lg => lg.Owner)
        //    .WithMany(u => u.CreatedGoals)
        //    .HasForeignKey(lg => lg.OwnerId)
        //    .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<LearningGoal>()
        //    .HasMany(lg => lg.Projects)
        //    .WithMany(p => p.Goals);

        //modelBuilder.Entity<LearningGoal>()
        //    .HasMany(lg => lg.UserGoals)
        //    .WithOne(ug => ug.Goal)
        //    .HasForeignKey(ug => ug.GoalId);

        //modelBuilder.Entity<LearningGoal>()
        //    .HasMany(lg => lg.GoalVertices)
        //    .WithOne(cv => cv.Cursus)
        //    .HasForeignKey(cv => cv.CursusId);

        // Project relationships
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Owner)
            .WithMany(u => u.CreatedProjects)
            .HasForeignKey(p => p.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<Project>()
        //    .HasMany(p => p.Rubrics)
        //    .WithOne(r => r.Project)
        //    .HasForeignKey(r => r.ProjectId);

        //modelBuilder.Entity<Project>()
        //    .HasMany(p => p.Tags)
        //    .WithMany(t => t.Projects);

        //modelBuilder.Entity<Project>()
        //    .HasMany(p => p.UserProjects)
        //    .WithOne(up => up.Project)
        //    .HasForeignKey(up => up.ProjectId);

        //modelBuilder.Entity<Project>()
        //    .HasOne(p => p.GitInfo)
        //    .WithMany(gi => gi.Projects)
        //    .HasForeignKey(p => p.GitInfoId);

        //// User relationships
        //modelBuilder.Entity<User>()
        //    .HasMany(u => u.CreatedCursus)
        //    .WithOne(c => c.Owner)
        //    .HasForeignKey(c => c.OwnerId);

        //modelBuilder.Entity<User>()
        //    .HasMany(u => u.CreatedGoals)
        //    .WithOne(g => g.Owner)
        //    .HasForeignKey(g => g.OwnerId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedProjects)
            .WithOne(p => p.Owner)
            .HasForeignKey(p => p.OwnerId);

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
    public DbSet<Event> Events { get; set; }
    public DbSet<EventAction> EventActions { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Cursus> Cursi { get; set; }
    public DbSet<CursusVertex> CursusVertices { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<LearningGoal> LearningGoals { get; set; }
    public DbSet<UserGoal> UserGoals { get; set; }
    public DbSet<UserProject> UserProject { get; set; }
    public DbSet<Rubric> Rubric { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Git> GitInfo { get; set; }
#nullable restore
}