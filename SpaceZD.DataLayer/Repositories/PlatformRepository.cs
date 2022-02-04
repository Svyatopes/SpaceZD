using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class PlatformRepository : BaseRepository, IRepositorySoftDelete<Platform>
{
    public PlatformRepository(VeryVeryImportantContext context) : base(context) { }

    public Platform? GetById(int id) => _context.Platforms.FirstOrDefault(x => x.Id == id);

    public IEnumerable<Platform> GetList(bool includeAll = false) => _context.Platforms.Where(p => !p.IsDeleted || includeAll).ToList();

    public void Add(Platform platform)
    {
        _context.Platforms.Add(platform);
        _context.SaveChanges();
    }

    public bool Update(Platform platform)
    {
        var platformInDb = GetById(platform.Id);

        if (platformInDb == null)
            return false;

        platformInDb.Number = platform.Number;

        _context.SaveChanges();
        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var platformInDb = GetById(id);

        if (platformInDb == null)
            return false;

        platformInDb.IsDeleted = isDeleted;

        _context.SaveChanges();
        return true;
    }
}