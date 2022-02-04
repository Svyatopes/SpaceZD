using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class StationRepository
    {
        private readonly VeryVeryImportantContext _context;
        public StationRepository() => _context = new VeryVeryImportantContext();
        public Station? GetEntity(int id) => _context.Stations.FirstOrDefault(s => s.Id == id);

        public void Add(Station station)
        {
            _context.Stations.Add(station);
            _context.SaveChanges();
        }

        public IEnumerable<Station> GetListEntity() => _context.Stations.Where(s => !s.IsDeleted).ToList();

        public bool UpdateRouteTransit(int id, bool isDeleted)
        {
            var entity = GetEntity(id);
            if (entity is null)
                return false;

            entity.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateRouteTransit(Station station)
        {
            var entity = GetEntity(station.Id);
            if (entity is null)
                return false;
            entity.Name = station.Name;

            _context.SaveChanges();
            return true;
        }
    }
}
