using JobTrackr.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTrackr.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(user => user.Applications)
            .WithOne(application => application.User)
            .HasForeignKey(application => application.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}