using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IRouteService : IDeleteRestoreUpdate<RouteModel>, IGetByIdWithUserId<RouteModel>, IAddWithUserId<RouteModel>
{
    List<RouteModel> GetList(int userId);
    List<RouteModel> GetListDeleted(int userId);
}