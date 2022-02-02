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

    public Carriage? GetEntity(int id) => _context.Carriages.FirstOrDefault(c => c.Id == id);

    public IEnumerable<Carriage> GetListEntity() => _context.Carriages.Where(t => !t.IsDeleted).ToList();

    public bool Delete(int id)
    {
        var entity = GetEntity(id);
        if (entity is null)
            return false;

        entity.IsDeleted = true;
        _context.SaveChanges();
        return true;
    }

    public bool Update(Carriage carriage)
    {
        var entity = GetEntity(carriage.Id);
        if (entity is null)
            return false;

        entity.Number = carriage.Number;
        entity.Train = carriage.Train;
        entity.Type = carriage.Type;

        _context.SaveChanges();

        return true;
    }
}