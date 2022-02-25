using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IRouteTransitService
    {
        int Add(int userId, RouteTransitModel routeTransit);
        void Delete(int userId, int id);
        RouteTransitModel GetById(int userId, int id);
        List<RouteTransitModel> GetList(int userId);
        List<RouteTransitModel> GetListDeleted(int userId);
        void Restore(int userId, int id);
        void Update(int userId, int id, RouteTransitModel routeTransit);
    }
}