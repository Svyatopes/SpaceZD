using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories;

public class PlatformMaintenanceRepository
{
    private readonly VeryVeryImportantContext _context;
    public PlatformMaintenanceRepository() => _context = VeryVeryImportantContext.GetInstance();

    public void Add(PlatformMaintenance platformMaintenance)
    {
        _context.PlatformMaintenances.Add(platformMaintenance);
        _context.SaveChanges();
    }

    public PlatformMaintenance? GetEntity(int id) => _context.PlatformMaintenances.FirstOrDefault(p => p.Id == id);

    public IEnumerable<PlatformMaintenance> GetListEntity(bool includeAll = false) => _context.PlatformMaintenances.Where(p => !p.IsDeleted || includeAll).ToList();

    public bool Update(int id, bool isDeleted)
    {
        var entity = GetEntity(id);
        if (entity is null)
            return false;

        entity.IsDeleted = isDeleted;

        _context.SaveChanges();

        return true;
    }

    public bool Update(PlatformMaintenance platformMaintenance)
    {
        var entity = GetEntity(platformMaintenance.Id);
        if (entity is null)
            return false;

        entity.Platform = platformMaintenance.Platform;
        entity.StartTime = platformMaintenance.StartTime;
        entity.EndTime = platformMaintenance.EndTime;

        _context.SaveChanges();

        return true;
    }
}