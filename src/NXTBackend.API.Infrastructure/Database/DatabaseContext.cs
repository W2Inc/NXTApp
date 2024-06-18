using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using NXTBackend.API.Domain.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Interceptors;

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
        //modelBuilder.Entity<Feature>()
        // .Property(s => s.CreatedAt)
        // .HasDefaultValueSql("GETDATE()");

        //modelBuilder.Entity<Feature>()
        //    .Property(s => s.UpdatedAt)
        //    .HasDefaultValueSql("GETDATE()");


        //modelBuilder.Entity<User>()
        //    .HasOne(u => u.Details)
        //    .WithOne(d => d.User)
        //    .HasForeignKey<Details>(d => d.UserId);

        //modelBuilder.Entity<User>()
        //    .HasMany(u => u.Comments)
        //    .WithOne(c => c.User)
        //    .HasForeignKey(c => c.UserId);

        //modelBuilder.Entity<Details>()
        //    .HasMany(d => d.Features)
        //    .WithMany(f => f.Details)
        //    .UsingEntity(j => j.ToTable("tbl_user_details_features"));

        //modelBuilder.Entity<EventAction>()
        //    .HasKey(ea => new { ea.UserId, ea.EventId });
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