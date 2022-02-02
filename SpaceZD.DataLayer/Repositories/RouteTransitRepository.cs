using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class RouteTransitRepository
    {
        public RouteTransit GetRouteTransitById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var routetransit = context.RouteTransits.FirstOrDefault(rt => rt.Id == id);

            return routetransit;
        }

        public List<RouteTransit> GetRouteTransits()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var routetransits = context.RouteTransits.ToList();

            return routetransits;
        }

        public void AddRouteTransit(RouteTransit routetransit)
        {
            var context = VeryVeryImportantContext.GetInstance();

            context.RouteTransits.Add(routetransit);

            context.SaveChanges();
        }

        public void DeleteRouteTransit(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var routetransit = context.RouteTransits.FirstOrDefault(rt => rt.Id == id);

            routetransit.IsDeleted = true;

            context.SaveChanges();
        }

        public void UpdateRouteTransit(RouteTransit routetransit)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var routetransitDB = GetRouteTransitById(routetransit.Id);

            routetransitDB.Transit = routetransit.Transit;
            routetransitDB.DepartingTime = routetransit.DepartingTime;
            routetransitDB.ArrivalTime = routetransit.ArrivalTime;
            routetransitDB.IsDeleted = routetransit.IsDeleted;
            routetransitDB.Route = routetransit.Route;

            context.SaveChanges();
        }
    }
}
