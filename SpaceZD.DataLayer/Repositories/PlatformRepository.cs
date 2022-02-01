using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class PlatformRepository
    {
        public Platform GetPlatformById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var platform = context.Platforms.Include(p => p.Station)
                                            .FirstOrDefault(o => o.Id == id);
            return platform;
        }


        public List<Platform> GetPlatforms()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var platforms = context.Platforms.ToList();
            return platforms;
        }

        public void AddPlatform(Platform platform)
        {
            var context = VeryVeryImportantContext.GetInstance();
            context.Platforms.Add(platform);
            context.SaveChanges();
        }

        public void EditPlatform(Platform platform)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var platformInDB = GetPlatformById(platform.Id);

            if (platformInDB == null)
                throw new Exception($"Not found Platform with {platform.Id} to edit");

            if (platformInDB.Number != platform.Number)
                platformInDB.Number = platform.Number;

            if (platform.Station != null && platformInDB.Station.Id != platform.Station.Id)
                platformInDB.Station = platform.Station;

            context.SaveChanges();
        }

        public void DeletePlatform(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var platformInDB = context.Platforms.FirstOrDefault(o => o.Id == id);

            if (platformInDB == null)
                throw new Exception($"Not found Platform with {id} to delete");

            platformInDB.IsDeleted = true;

            context.SaveChanges();
        }

        public void RestorePlatform(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var platformInDB = context.Platforms.FirstOrDefault(o => o.Id == id);

            if (platformInDB == null)
                throw new Exception($"Not found Platform with {id} to restore");

            platformInDB.IsDeleted = false;

            context.SaveChanges();
        }

    }
}
