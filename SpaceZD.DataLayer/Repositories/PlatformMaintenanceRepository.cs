using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class PlatformMaintenanceRepository : BaseRepository, IPlatformMaintenanceRepository
{
    public PlatformMaintenanceRepository(VeryVeryImportantContext context) : base(context) { }

    public PlatformMaintenance? GetById(int id) =>
        _context.PlatformMaintenances
                .Include(p => p.Platform)
                .FirstOrDefault(p => p.Id == id);

    public List<PlatformMaintenance> GetListByStationId(int stationId, bool includeAll = false) =>
        _context.PlatformMaintenances
        .Include(p => p.Platform)
        .Where(p => (!p.IsDeleted || includeAll) && p.Platform.StationId == stationId)
        .ToList();

    public int Add(PlatformMaintenance platformMaintenance)
    {
        _context.PlatformMaintenances.Add(platformMaintenance);
        _context.SaveChanges();
        return platformMaintenance.Id;
    }

    public void Update(PlatformMaintenance oldPlatformMaintenance, PlatformMaintenance newPlatformMaintenance)
    {
        oldPlatformMaintenance.Platform = newPlatformMaintenance.Platform;
        oldPlatformMaintenance.StartTime = newPlatformMaintenance.StartTime;
        oldPlatformMaintenance.EndTime = newPlatformMaintenance.EndTime;

        _context.SaveChanges();
    }

    public void Update(PlatformMaintenance oldPlatformMaintenance, bool isDeleted)
    {
        oldPlatformMaintenance.IsDeleted = isDeleted;

        _context.SaveChanges();

    }
}