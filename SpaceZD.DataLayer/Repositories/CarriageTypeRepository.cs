using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories;

public class CarriageTypeRepository
{
    private readonly VeryVeryImportantContext _context;
    public CarriageTypeRepository() => _context = VeryVeryImportantContext.GetInstance();

    public void Add(CarriageType carriageType)
    {
        _context.CarriageTypes.Add(carriageType);
        _context.SaveChanges();
    }

    public CarriageType? GetEntity(int id) => _context.CarriageTypes.FirstOrDefault(c => c.Id == id);

    public IEnumerable<CarriageType> GetListEntity() => _context.CarriageTypes.Where(t => !t.IsDeleted).ToList();

    public bool Delete(int id)
    {
        var entity = GetEntity(id);
        if (entity is null)
            return false;

        entity.IsDeleted = true;
        _context.SaveChanges();

        return true;
    }

    public bool Update(CarriageType carriageType)
    {
        var entity = GetEntity(carriageType.Id);
        if (entity is null)
            return false;

        entity.Name = carriageType.Name;
        entity.NumberOfSeats = carriageType.NumberOfSeats;

        _context.SaveChanges();

        return true;
    }
}