using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class CarriageRepository : BaseRepository, IRepositorySoftDelete<Carriage>
{
    public CarriageRepository(VeryVeryImportantContext context) : base(context) { }

    public Carriage? GetById(int id) =>
        _context.Carriages
                .Include(c => c.Type)
                .FirstOrDefault(c => c.Id == id);

    public List<Carriage> GetList(bool includeAll = false) => _context.Carriages.Where(c => !c.IsDeleted || includeAll).ToList();

    public int Add(Carriage carriage)
    {
        _context.Carriages.Add(carriage);
        _context.SaveChanges();
        return carriage.Id;
    }

    public bool Update(Carriage carriage)
    {
        var entity = GetById(carriage.Id);
        if (entity is null)
            return false;

        entity.Number = carriage.Number;
        entity.Train = carriage.Train;
        entity.Type = carriage.Type;

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