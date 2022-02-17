using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripRepository : BaseRepository, IRepositorySoftDelete<Trip>
{
    public TripRepository(VeryVeryImportantContext context) : base(context) {}

    public Trip? GetById(int id) =>
        _context.Trips
                .Include(t => t.Route)
                .Include(t => t.Train)
                .Include(t => t.Stations)
                .FirstOrDefault(t => t.Id == id);

    public List<Trip> GetList(bool includeAll = false) =>
        _context.Trips
                .Where(t => !t.IsDeleted || includeAll)
                .Include(t => t.Route)
                .Include(t => t.Train)
                .Include(t => t.Stations)
                .ToList();

    public int Add(Trip trip)
    {
        _context.Trips.Add(trip);
        _context.SaveChanges();
        return trip.Id;
    }

    public void Update(Trip entityToEdit, Trip newEntity)
    {
        entityToEdit.Train = newEntity.Train;

        _context.SaveChanges();
    }

    public void Update(Trip entity, bool isDeleted)
    {
        entity.IsDeleted = isDeleted;

        _context.SaveChanges();
    }
}