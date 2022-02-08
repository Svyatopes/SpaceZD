using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class StationRepository : BaseRepository, IRepositorySoftDelete<Station>
{
    public StationRepository(VeryVeryImportantContext context) : base(context) { }

    public Station? GetById(int id) =>
        _context.Stations
                .Include(s => s.Platforms)
                .FirstOrDefault(s => s.Id == id);

    public IEnumerable<Station> GetList(bool includeAll = false) => _context.Stations.Where(s => !s.IsDeleted || includeAll).ToList();

    public int Add(Station station)
    {
        _context.Stations.Add(station);
        _context.SaveChanges();
        return station.Id;
    }

    public bool Update(Station station)
    {
        var entity = GetById(station.Id);
        if (entity is null)
            return false;

        entity.Name = station.Name;

        _context.SaveChanges();
        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var entity = GetById(id);
        if (entity is null)
            return false;

        entity.IsDeleted = isDeleted;
        _context.SaveChanges();
        return true;
    }
}