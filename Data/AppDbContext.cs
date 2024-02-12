using JobOpeningBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace JobOpeningBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     // Configure the one-to-many relationship between Job and Location
    //     modelBuilder
    //         .Entity<Job>()
    //         .HasOne(j => j.Location)
    //         .WithMany(l => l.Jobs)
    //         .HasForeignKey(j => j.LocationId);

    //     // Configure the one-to-many relationship between Job and Department
    //     modelBuilder
    //         .Entity<Job>()
    //         .HasOne(j => j.Department)
    //         .WithMany(d => d.Jobs)
    //         .HasForeignKey(j => j.DepartmentId);

    //     // Configure the navigation property for the one-to-many relationship between Department and Job
    //     modelBuilder
    //         .Entity<Department>()
    //         .HasMany(d => d.Jobs)
    //         .WithOne(j => j.Department)
    //         .HasForeignKey(j => j.DepartmentId);

    //     // Configure the navigation property for the one-to-many relationship between Location and Job
    //     modelBuilder
    //         .Entity<Location>()
    //         .HasMany(d => d.Jobs)
    //         .WithOne(j => j.Location)
    //         .HasForeignKey(j => j.LocationId);
    // }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Location> Locations { get; set; }

    public DbSet<UserToken> UserToken { get; set; }
}
