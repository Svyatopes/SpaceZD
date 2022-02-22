using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class PlatformRepository : BaseRepository, IPlatformRepository
{
    public PlatformRepository(VeryVeryImportantContext context) : base(context) { }

    public Platform? GetById(int id) =>
        _context.Platforms
                .Include(x => x.Station)
                .FirstOrDefault(x => x.Id == id);

    public List<Platform> GetList(int stationId, bool includeAll = false) =>
        _context.Platforms
                .Include(p => p.Station)
                .Where(p => (!p.IsDeleted || includeAll) && p.Station.Id == stationId)
                .ToList();

    public int Add(Platform platform)
    {
        _context.Platforms.Add(platform);
        _context.SaveChanges();
        return platform.Id;
    }

    public void Update(Platform oldPlatform, Platform newPlatform)
    {
        oldPlatform.Number = newPlatform.Number;
        _context.SaveChanges();
    }

    public void Update(Platform platform, bool isDeleted)
    {
        platform.IsDeleted = isDeleted;
        _context.SaveChanges();
    }
}