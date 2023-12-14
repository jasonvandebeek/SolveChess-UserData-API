using Microsoft.EntityFrameworkCore;
using SolveChess.Logic.Models;

namespace SolveChess.DAL.Model;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .Property(u => u.Rating)
            .HasDefaultValue(300);

        modelBuilder.Entity<User>()
            .Property(u => u.ProfilePicture)
            .IsRequired(false);

    }

}