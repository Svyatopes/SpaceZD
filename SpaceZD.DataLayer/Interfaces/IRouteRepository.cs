using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface IRouteRepository : IRepositorySoftDelete<Route>
{
    void AddRouteTransitForRoute(Route route, RouteTransit routeTransit);
}