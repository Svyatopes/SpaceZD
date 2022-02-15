using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TrainRepository : BaseRepository, IRepositorySoftDelete<Train>
{
    public TrainRepository(VeryVeryImportantContext context) : base(context) { }

    public Train? GetById(int id) =>
        _context.Trains
                .Include(t => t.Carriages)
                .FirstOrDefault(t => t.Id == id);

    public List<Train> GetList(bool includeAll = false) => _context.Trains.Where(c => !c.IsDeleted || includeAll).ToList();

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