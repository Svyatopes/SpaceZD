using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TransitRepository : BaseRepository, IRepositorySoftDelete<Transit>
{
    public TransitRepository(VeryVeryImportantContext context) : base(context) { }

    public Transit? GetById(int id) =>
        _context.Transits
                .Include(c => c.StartStation)
                .Include(c => c.EndStation)
                .FirstOrDefault(c => c.Id == id);

    public IEnumerable<Transit> GetList(bool includeAll = false) => _context.Transits.Where(t => !t.IsDeleted || includeAll).ToList();

    public int Add(Transit transit)
    {
        _context.Transits.Add(transit);
        _context.SaveChanges();
        return transit.Id;
    }

    public bool Update(Transit transit)
    {
        var entity = GetById(transit.Id);
        if (entity == null) return false;

        entity.StartStation = transit.StartStation;
        entity.EndStation = transit.EndStation;
        entity.Price = transit.Price;
        entity.RouteTransit = transit.RouteTransit;

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