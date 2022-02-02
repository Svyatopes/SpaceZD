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
            Save();
        }

        public Transit? GetEntity(int id) => _context.Transits.FirstOrDefault(c => c.Id == id);

        public IEnumerable<Transit> GetListEntity() => _context.Transits.ToList();

        public bool Delete(int id)
        {
            var entity = GetEntity(id);

            if (entity == null) return false;

            entity.IsDeleted = true;
            Save();

            return true;
        }

        public bool Recover(int id)
        {
            var entity = GetEntity(id);

            if (entity == null) return false;

            entity.IsDeleted = false;
            Save();

            return true;
        }

        public bool Update(Transit transit)
        {
            var entity = GetEntity(transit.Id);
            if (entity == null) return false;

            entity.StartStation = transit.StartStation;
            entity.EndStation = transit.EndStation;
            entity.Price = transit.Price;
            entity.RouteTransit = transit.RouteTransit;

            Save();
            return true;
        }

        private void Save() => _context.SaveChanges();
    }
}
