using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TripStationRepository : BaseRepository, ITripStationRepository
{
    public TripStationRepository(VeryVeryImportantContext context) : base(context) {}

    public TripStation? GetById(int id) =>
        _context.TripStations
                .Include(c => c.Platform)
                .Include(c => c.Station)
                .FirstOrDefault(c => c.Id == id);

    public List<TripStation> GetList() =>
        _context.TripStations
                .Include(c => c.Platform)
                .Include(c => c.Station)
                .ToList();

    public void Update(TripStation entityToEdit, TripStation newEntity)
    {
        entityToEdit.Platform = newEntity.Platform;
        entityToEdit.ArrivalTime = newEntity.ArrivalTime;
        entityToEdit.DepartingTime = newEntity.DepartingTime;

        _context.SaveChanges();
    }
}