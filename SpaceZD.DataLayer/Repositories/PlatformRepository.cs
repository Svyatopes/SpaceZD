using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories
{
    public class PlatformRepository : BaseRepository, IRepository<Platform>, ISoftDelete<Platform>
    {
        public Platform GetEntity(int id) => _context.Platforms.FirstOrDefault(x => x.Id == id);


        public List<Platform> GetList(bool includeAll = false) => _context.Platforms.Where(p => !p.IsDeleted || includeAll).ToList();


        public void Add(Platform platform)
        {
            _context.Platforms.Add(platform);
            _context.SaveChanges();
        }

        public bool Update(Platform platform)
        {
            var platformInDB = GetEntity(platform.Id);

            if (platformInDB == null)
                return false;

            platformInDB.Number = platform.Number;

            _context.SaveChanges();
            return true;
        }

        public bool Update(int id, bool isDeleted)
        {
            var platformInDB = GetEntity(id);

            if (platformInDB == null)
                return false;

            platformInDB.IsDeleted = isDeleted;

            _context.SaveChanges();
            return true;
        }

    }
}
