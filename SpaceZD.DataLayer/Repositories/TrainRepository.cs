using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TrainRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<Train>
{
    public TrainRepository(VeryVeryImportantContext context) : base(context) { }

    public Train? GetById(int id)
    {
        var entity = _context.Trains
                             .Include(t => t.Carriages.Where(c => !c.IsDeleted))
                             .FirstOrDefault(t => t.Id == id);
        if (entity is null)
            return null;
        entity.Carriages = entity.Carriages.Where(c => !c.IsDeleted).ToList();
        return entity;
    }

    public List<Train> GetList(bool includeAll = false)
    {
        var entities = _context.Trains
                               .Include(t => t.Carriages.Where(c => !c.IsDeleted))
                               .Where(t => !t.IsDeleted || includeAll).ToList();

        foreach (var train in entities)
            train.Carriages = train.Carriages.Where(c => !c.IsDeleted).ToList();
        return entities;      


    }

    public int Add(Train train)
    {
        _context.Trains.Add(train);
        _context.SaveChanges();
        return train.Id;
    }

    public void Update(Train trainOld, Train trainNew)
    {
        trainOld.Carriages = trainNew.Carriages;
        _context.SaveChanges();
        
    }    

    public void Update(Train train, bool isDeleted)
    {
        
        train.IsDeleted = isDeleted;
        _context.SaveChanges();

    }
    
}