using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripRepository : BaseRepository, IRepositorySoftDelete<Trip>
{
    public TripRepository(VeryVeryImportantContext context) : base(context) { }

    public Trip? GetById(int id) =>
        _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Train)
                .Include(t => t.Stations)
                .FirstOrDefault(t => t.Id == id);

    public IEnumerable<Trip> GetList(bool includeAll = false) => _context.Trips.Where(t => !t.IsDeleted || includeAll).ToList();

    public int Add(Trip trip)
    {
        _context.Trips.Add(trip);
        _context.SaveChanges();
        return trip.Id;
    }

    public bool Update(Trip trip)
    {
        var entity = GetById(trip.Id);
        if (entity == null) return false;

        entity.Train = trip.Train;
        entity.Route = trip.Route;
        entity.Stations = trip.Stations;
        entity.StartTime = trip.StartTime;

        _context.SaveChanges();
        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var entity = GetById(id);
        if (entity == null) return false;

        entity.IsDeleted = isDeleted;

        _context.SaveChanges();
        return true;
    }
}