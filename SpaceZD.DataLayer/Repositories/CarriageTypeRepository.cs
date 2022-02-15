using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class CarriageTypeRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<CarriageType>
{
    public CarriageTypeRepository(VeryVeryImportantContext context) : base(context) { }

    public CarriageType? GetById(int id) => _context.CarriageTypes.FirstOrDefault(c => c.Id == id);

    public List<CarriageType> GetList(bool includeAll = false) => _context.CarriageTypes.Where(c => !c.IsDeleted || includeAll).ToList();

    public int Add(CarriageType carriageType)
    {
        _context.CarriageTypes.Add(carriageType);
        _context.SaveChanges();
        return carriageType.Id;
    }

    public void Update(CarriageType carriageTypeOld, CarriageType carriageTypeUpdate)
    {
        carriageTypeOld.Name = carriageTypeUpdate.Name;
        carriageTypeOld.NumberOfSeats = carriageTypeUpdate.NumberOfSeats;

        _context.SaveChanges();
    }

    public void Update(CarriageType carriageType, bool isDeleted)
    {
        carriageType.IsDeleted = isDeleted;

        _context.SaveChanges();
    }
}