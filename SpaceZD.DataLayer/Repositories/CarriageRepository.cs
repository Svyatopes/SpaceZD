using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories;

public class CarriageRepository
{
    private readonly VeryVeryImportantContext _context;
    public CarriageRepository() => _context = VeryVeryImportantContext.GetInstance();

    public void Add(Carriage carriage)
    {
        _context.Carriages.Add(carriage);
        _context.SaveChanges();
    }

    public Carriage GetEntity(int id)
    {
        var entity = _context.Carriages.FirstOrDefault(c => c.Id == id);
        if (entity == null)
            throw new Exception($"Carriage c Id = {id} не найден");

        return entity;
    }

    public IEnumerable<Carriage> GetListEntity() => _context.Carriages.ToList();

    public void Delete(int id)
    {
        GetEntity(id).IsDeleted = true;
        
        _context.SaveChanges();
    }

    public void Update(Carriage carriage)
    {
        var entity = GetEntity(carriage.Id);
        
        entity.Number    = carriage.Number;
        entity.Tickets   = carriage.Tickets;
        entity.Train     = carriage.Train;
        entity.Type      = carriage.Type;
        entity.IsDeleted = carriage.IsDeleted;
        
        _context.SaveChanges();
    }
}