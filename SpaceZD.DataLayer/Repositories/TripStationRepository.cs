using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripStationRepository : BaseRepository, IRepository<TripStation>
{
    public TripStationRepository(VeryVeryImportantContext context) : base(context) { }

    public TripStation? GetById(int id) => _context.TripStations.FirstOrDefault(c => c.Id == id);

    public IEnumerable<TripStation> GetList() => _context.TripStations.ToList();

    public void Add(TripStation tripStation)
    {
        _context.TripStations.Add(tripStation);
        _context.SaveChanges();
    }

    public bool Update(TripStation tripStation)
    {
        var entity = GetById(tripStation.Id);
        if (entity == null) return false;

        entity.Platform = tripStation.Platform;
        entity.ArrivalTime = tripStation.ArrivalTime;
        entity.DepartingTime = tripStation.DepartingTime;

        _context.SaveChanges();
        return true;
    }
}