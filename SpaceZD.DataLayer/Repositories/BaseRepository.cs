using SpaceZD.DataLayer.DbContextes;

namespace SpaceZD.DataLayer.Repositories
{
    public class BaseRepository
    {
        protected readonly VeryVeryImportantContext _context;
        public BaseRepository() => _context = VeryVeryImportantContext.GetInstance();
    }
}
