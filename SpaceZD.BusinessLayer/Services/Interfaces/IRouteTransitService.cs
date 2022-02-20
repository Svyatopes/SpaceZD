using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    internal interface IRouteTransitService
    {
        int Add(RouteTransitModel routeTransit);
        void Delete(int id);
        RouteTransitModel GetById(int id);
        List<RouteTransitModel> GetList(bool allIncluded);
        void Restore(int id);
        void Update(int id, RouteTransitModel routeTransit);
    }
}