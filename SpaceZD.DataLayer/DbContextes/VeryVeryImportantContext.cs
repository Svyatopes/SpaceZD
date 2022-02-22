using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Extensions;

namespace SpaceZD.DataLayer.DbContextes;

public class VeryVeryImportantContext : DbContext
{
    public VeryVeryImportantContext(DbContextOptions<VeryVeryImportantContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.RemovePluralizingTableNameConvention();

        modelBuilder.MaxLengthOfAllStringsInTables(100);

        modelBuilder.IsDeletedPropertyFalse();

        modelBuilder.Entity<Order>()
                    .HasOne(e => e.StartStation)
                    .WithMany(e => e.OrdersWithStartStation);
        modelBuilder.Entity<Order>()
                    .HasOne(e => e.EndStation)
                    .WithMany(e => e.OrdersWithEndStation);

        modelBuilder.Entity<Route>()
                    .HasOne(e => e.StartStation)
                    .WithMany(e => e.RoutesWithStartStation);
        modelBuilder.Entity<Route>()
                    .HasOne(e => e.EndStation)
                    .WithMany(e => e.RoutesWithEndStation);

        modelBuilder.Entity<Transit>()
                    .HasOne(e => e.StartStation)
                    .WithMany(e => e.TransitsWithStartStation);
        modelBuilder.Entity<Transit>()
                    .HasOne(e => e.EndStation)
                    .WithMany(e => e.TransitsWithEndStation);

        modelBuilder.Entity<Ticket>()
                    .Property(p => p.Price)
                    .HasPrecision(9, 2);

        modelBuilder.Entity<Transit>()
                    .Property(p => p.Price)
                    .HasPrecision(9, 2);

        modelBuilder.Entity<CarriageType>()
                    .Property(p => p.PriceFactor)
                    .HasPrecision(6, 3)
                    .HasDefaultValue(1);

        modelBuilder.DisableCascadeDelete();

        modelBuilder.Entity<Transit>()
                    .HasIndex(p => new { p.StartStationId, p.EndStationId })
                    .IsUnique();

        modelBuilder.Entity<Platform>()
                    .HasIndex(p => new { p.Number, p.StationId })
                    .IsUnique();
    }

    public DbSet<Carriage> Carriages { get; set; }
    public DbSet<CarriageType> CarriageTypes { get; set; }
    public DbSet<PlatformMaintenance> PlatformMaintenances { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<RouteTransit> RouteTransits { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<Transit> Transits { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<TripStation> TripStations { get; set; }
    public DbSet<User> Users { get; set; }
}