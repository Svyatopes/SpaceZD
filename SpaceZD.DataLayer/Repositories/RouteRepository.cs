using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class RouteRepository
    {
        public Route GetRouteById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var route = context.Routes.FirstOrDefault(r => r.Id == id);

            return route;
        }

        public List<Route> GetRoutes()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var routes = context.Routes.ToList();

            return routes;
        }

        public void AddRoute(Route route)
        {
            var context = VeryVeryImportantContext.GetInstance();

            context.Routes.Add(route);

            context.SaveChanges();
        }

        public void DeleteRoute(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var route = context.Routes.FirstOrDefault(r => r.Id == id);

            route.IsDeleted = true;

            context.SaveChanges();
        }

        public void UpdateRoute(Route route)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var routeDB = GetRouteById(route.Id);

            routeDB.Code = route.Code;
            routeDB.Transits = route.Transits;
            routeDB.StartTime = route.StartTime;
            routeDB.StartStation = route.StartStation;
            routeDB.EndStation = route.EndStation;
            routeDB.IsDeleted = route.IsDeleted;
            routeDB.Trips = route.Trips;

            context.SaveChanges();
        }
    }
}
