using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TransitRepository
    {
        private readonly VeryVeryImportantContext _context;
        public TransitRepository() => _context = VeryVeryImportantContext.GetInstance();

        public void Add(Transit transit)
        {
            _context.Transits.Add(transit);
            _context.SaveChanges();
        }

        public Transit? GetById(int id) => _context.Transits.FirstOrDefault(c => c.Id == id);

        public IEnumerable<Transit> GetList(bool includeAll = false) => _context.Transits.Where(t => !t.IsDeleted || includeAll).ToList();

        public bool Update(int id, bool isDeleted)
        {
            var entity = GetById(id);
            if (entity == null) return false;

            entity.IsDeleted = isDeleted;

            _context.SaveChanges();
            return true;
        }

        public bool Update(Transit transit)
        {
            var entity = GetById(transit.Id);
            if (entity == null) return false;

            entity.StartStation = transit.StartStation;
            entity.EndStation = transit.EndStation;
            entity.Price = transit.Price;
            entity.RouteTransit = transit.RouteTransit;

            _context.SaveChanges();
            return true;
        }
    }
}
