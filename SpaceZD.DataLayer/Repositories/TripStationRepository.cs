using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripStationRepository : BaseRepository, IRepository<TripStation>
{
    public TripStationRepository(VeryVeryImportantContext context) : base(context) { }

    public TripStation? GetById(int id) =>
        _context.TripStations
                .Include(c => c.Platform)
                .Include(c => c.Station)
                .FirstOrDefault(c => c.Id == id);

    public IEnumerable<TripStation> GetList() => _context.TripStations.ToList();

    public int Add(TripStation tripStation)
    {
        _context.TripStations.Add(tripStation);
        _context.SaveChanges();
        return tripStation.Id;
    }

    public void Update(TripStation entityToEdit, TripStation newEntity)
    {
        entityToEdit.Platform = newEntity.Platform;
        entityToEdit.ArrivalTime = newEntity.ArrivalTime;
        entityToEdit.DepartingTime = newEntity.DepartingTime;

        _context.SaveChanges();
    }
}