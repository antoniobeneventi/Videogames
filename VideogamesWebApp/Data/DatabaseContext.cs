using Microsoft.EntityFrameworkCore;
using VideogamesWebApp.Models;

namespace GamesDataAccess;

public class DatabaseContext : DbContext
{
    public DbSet<Game> Games { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().HasKey(g => g.GameId);
        modelBuilder.Entity<Game>().Property(g => g.GameName).IsRequired();
        modelBuilder.Entity<Game>().Property(g => g.GameDescription).IsRequired();
        modelBuilder.Entity<Game>().Property(g => g.GameTags);
    }
}
