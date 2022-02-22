using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IRouteService
{
    RouteModel GetById(int userId, int id);
    List<RouteModel> GetList(int userId);
    List<RouteModel> GetListDeleted(int userId);
    int Add(int userId, RouteModel routeModel);
    void Delete(int userId, int id);
    void Restore(int userId, int id);
    void Update(int userId, int id, RouteModel routeModel);
}