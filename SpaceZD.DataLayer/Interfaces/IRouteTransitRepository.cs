using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public interface IRouteTransitRepository
    {
        int Add(RouteTransit routeTransit);
        RouteTransit? GetById(int id);
        List<RouteTransit> GetList(int routeId, bool includeAll = false);
        void Update(RouteTransit routeTransit, bool isDeleted);
        void Update(RouteTransit oldRouteTransit, RouteTransit newRouteTransit);
    }
}