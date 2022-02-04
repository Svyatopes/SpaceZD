using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class RouteRepository
    {
        private readonly VeryVeryImportantContext _context;
        public RouteRepository() => _context = new VeryVeryImportantContext();
        public Route? GetEntity(int id) => _context.Routes.FirstOrDefault(r => r.Id == id);

        public void Add(Route route)
        {
            _context.Routes.Add(route);
            _context.SaveChanges();
        }

        public IEnumerable<Route> GetListEntity() => _context.Routes.Where(r => !r.IsDeleted).ToList();


        public bool UpdateRouteTransit(int id, bool isDeleted)
        {
            var entity = GetEntity(id);
            if (entity is null)
                return false;

            entity.IsDeleted = true;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateRouteTransit(Route route)
        {
            var entity = GetEntity(route.Id);
            if (entity is null)
                return false;
            entity.Code = route.Code;
            entity.StartTime = route.StartTime;
            entity.StartStation = route.StartStation;
            entity.EndStation = route.EndStation;

            _context.SaveChanges();
            return true;
        }

    }
}
