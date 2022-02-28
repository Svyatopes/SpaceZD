using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IRouteTransitService : IDeleteRestoreUpdate<RouteTransitModel>, IGetByIdWithUserId<RouteTransitModel>, IAddWithUserId<RouteTransitModel>
{
    List<RouteTransitModel> GetListByRouteId(int userId, int routeId);
    List<RouteTransitModel> GetListByRouteIdDeleted(int userId, int routeId);
}