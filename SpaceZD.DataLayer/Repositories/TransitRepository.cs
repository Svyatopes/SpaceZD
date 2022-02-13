using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TransitRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<Transit>
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

    public void Update(Transit entityToEdit, Transit newEntity)
    {
        entityToEdit.StartStation = newEntity.StartStation;
        entityToEdit.EndStation = newEntity.EndStation;
        entityToEdit.Price = newEntity.Price;
        entityToEdit.RouteTransit = newEntity.RouteTransit;

        _context.SaveChanges();
    }

    public void Update(Transit entity, bool isDeleted)
    {
        entity.IsDeleted = isDeleted;

        _context.SaveChanges();
    }
}