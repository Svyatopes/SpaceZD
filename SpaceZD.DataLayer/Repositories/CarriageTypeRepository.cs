using Microsoft.EntityFrameworkCore;
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

    public CarriageType GetEntity(int id)
    {
        var entity = _context.CarriageTypes.FirstOrDefault(c => c.Id == id);
        if (entity == null)
            throw new Exception($"CarriageType c Id = {id} не найден");

        return entity;
    }

    public IEnumerable<CarriageType> GetListEntity() => _context.CarriageTypes.Where(t => !t.IsDeleted).ToList();

    public void Delete(int id)
    {
        var entity = GetEntity(id);
        foreach (var carriage in entity.Carriages)
            carriage.IsDeleted = true;
        entity.IsDeleted = true;
        
        _context.SaveChanges();
    }

    public void Update(CarriageType carriageType)
    {
        var entity = GetEntity(carriageType.Id);
        
        entity.Name          = carriageType.Name;
        entity.Carriages     = carriageType.Carriages;
        entity.NumberOfSeats = carriageType.NumberOfSeats;
        entity.IsDeleted     = carriageType.IsDeleted;

        _context.Entry(entity).State = EntityState.Modified;

        _context.SaveChanges();
    }
}