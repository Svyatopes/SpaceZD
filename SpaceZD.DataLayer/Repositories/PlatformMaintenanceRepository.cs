using Microsoft.EntityFrameworkCore;
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

    public PlatformMaintenance GetEntity(int id)
    {
        var entity = _context.PlatformMaintenances.FirstOrDefault(p => p.Id == id);
        if (entity == null)
            throw new Exception($"PlatformMaintenance c Id = {id} не найден");

        return entity;
    }

    public IEnumerable<PlatformMaintenance> GetListEntity() => _context.PlatformMaintenances.Where(t => !t.IsDeleted).ToList();

    public void Delete(int id)
    {
        GetEntity(id).IsDeleted = true;
        
        _context.SaveChanges();
    }

    public void Update(PlatformMaintenance platformMaintenance)
    {
        var entity = GetEntity(platformMaintenance.Id);

        entity.Platform  = platformMaintenance.Platform;
        entity.StartTime = platformMaintenance.StartTime;
        entity.EndTime   = platformMaintenance.EndTime;
        entity.IsDeleted = platformMaintenance.IsDeleted;
        
        _context.SaveChanges();
    }
}