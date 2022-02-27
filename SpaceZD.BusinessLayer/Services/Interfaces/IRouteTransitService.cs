using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IRouteTransitService
    {
        int Add(int userId, RouteTransitModel routeTransitModel);
        void Delete(int userId, int id);
        RouteTransitModel GetById(int userId, int id);
        List<RouteTransitModel> GetListByRoute(int userId, int routeId);
        List<RouteTransitModel> GetListByRouteDeleted(int userId, int routeId);
        void Restore(int userId, int id);
        void Update(int userId, int id, RouteTransitModel routeTransitModel);
    }
}