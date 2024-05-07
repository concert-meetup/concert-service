using concert.API.Models;
using Microsoft.EntityFrameworkCore;

namespace concert.API.Data;

public class ConcertDbContext : DbContext
{
    public ConcertDbContext(DbContextOptions<ConcertDbContext> options) : base(options)
    {
        
    }

    public DbSet<Concert> Concerts { get; set; }
    public DbSet<Venue> Venues { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Concert>()
            .ToTable("concerts");
        modelBuilder.Entity<Venue>()
            .ToTable("venues");
    }
}