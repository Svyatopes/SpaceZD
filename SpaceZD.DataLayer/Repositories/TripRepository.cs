using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<Trip>
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

    public void Update(Trip entityToEdit, Trip newEntity)
    {
        entityToEdit.Train = newEntity.Train;
        entityToEdit.Route = newEntity.Route;
        entityToEdit.Stations = newEntity.Stations;
        entityToEdit.StartTime = newEntity.StartTime;

        _context.SaveChanges();
    }

    public void Update(Trip entity, bool isDeleted)
    {
        entity.IsDeleted = isDeleted;

        _context.SaveChanges();
    }
}