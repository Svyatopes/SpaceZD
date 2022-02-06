using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class PlatformMaintenanceRepository : BaseRepository, IRepositorySoftDelete<PlatformMaintenance>
{
    public PlatformMaintenanceRepository(VeryVeryImportantContext context) : base(context) { }

    public PlatformMaintenance? GetById(int id) => _context.PlatformMaintenances.FirstOrDefault(p => p.Id == id);

    public IEnumerable<PlatformMaintenance> GetList(bool includeAll = false) => _context.PlatformMaintenances.Where(p => !p.IsDeleted || includeAll).ToList();

    public int Add(PlatformMaintenance platformMaintenance)
    {
        _context.PlatformMaintenances.Add(platformMaintenance);
        _context.SaveChanges();
        return platformMaintenance.Id;
    }

    public bool Update(PlatformMaintenance platformMaintenance)
    {
        var entity = GetById(platformMaintenance.Id);
        if (entity is null)
            return false;

        entity.Platform = platformMaintenance.Platform;
        entity.StartTime = platformMaintenance.StartTime;
        entity.EndTime = platformMaintenance.EndTime;

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