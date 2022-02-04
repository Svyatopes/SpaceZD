using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class RouteTransitRepository
    {
        private readonly VeryVeryImportantContext _context;
        public RouteTransitRepository() => _context = new VeryVeryImportantContext();
        public RouteTransit? GetEntity(int id) => _context.RouteTransits.FirstOrDefault(rt => rt.Id == id);

        public void Add(RouteTransit routetransit)
        {
            _context.RouteTransits.Add(routetransit);
            _context.SaveChanges();
        }

        public IEnumerable<RouteTransit> GetListEntity() => _context.RouteTransits.Where(rt => !rt.IsDeleted).ToList();

        public bool UpdateRouteTransit(int id, bool isDeleted)
        {
            var entity = GetEntity(id);
            if (entity is null)
                return false;

            entity.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateRouteTransit(RouteTransit routetransit)
        {
            var entity = GetEntity(routetransit.Id);
            if (entity is null)
                return false;
            entity.Transit = routetransit.Transit;
            entity.DepartingTime = routetransit.DepartingTime;
            entity.ArrivalTime = routetransit.ArrivalTime;
            entity.Route = routetransit.Route;

            _context.SaveChanges();
            return true;
        }
    }
}
