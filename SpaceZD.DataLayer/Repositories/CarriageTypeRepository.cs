using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class CarriageTypeRepository : BaseRepository, IRepositorySoftDelete<CarriageType>
{
    public CarriageTypeRepository(VeryVeryImportantContext context) : base(context) { }

    public CarriageType? GetById(int id) => _context.CarriageTypes.FirstOrDefault(c => c.Id == id);

    public IEnumerable<CarriageType> GetList(bool includeAll = false) => _context.CarriageTypes.Where(c => !c.IsDeleted || includeAll).ToList();

    public void Add(CarriageType carriageType)
    {
        _context.CarriageTypes.Add(carriageType);
        _context.SaveChanges();
    }

    public bool Update(CarriageType carriageType)
    {
        var entity = GetById(carriageType.Id);
        if (entity is null)
            return false;

        entity.Name = carriageType.Name;
        entity.NumberOfSeats = carriageType.NumberOfSeats;

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