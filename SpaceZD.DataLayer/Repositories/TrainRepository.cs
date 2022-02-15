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

    public bool Update(Train train)
    {
        var trainInDb = GetById(train.Id);

        if (trainInDb == null)
            return false;

        if (trainInDb.Carriages != null && trainInDb.Carriages != train.Carriages)
            trainInDb.Carriages = train.Carriages;

        _context.SaveChanges();
        return false;
    }

    public bool Update(int id, bool isDeleted)
    {
        var train = GetById(id);
        if (train is null)
            return false;

        train.IsDeleted = isDeleted;
        _context.SaveChanges();

        return true;
    }
}