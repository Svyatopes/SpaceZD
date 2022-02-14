using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class CarriageRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<Carriage>
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

    public void Update(Carriage oldCarriage, Carriage newCarriage)
    {
        oldCarriage.Number = newCarriage.Number;
        oldCarriage.Train = newCarriage.Train;
        oldCarriage.Type = newCarriage.Type;

        _context.SaveChanges();
    }

    public void Update(Carriage carriage, bool isDeleted)
    {
        carriage.IsDeleted = isDeleted;

        _context.SaveChanges();

    }
}