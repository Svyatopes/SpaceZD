using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IRouteService
{
    RouteModel GetById(int id);
    List<RouteModel> GetList();
    List<RouteModel> GetListDeleted();
    int Add(RouteModel routeModel);
    void Delete(int id);
    void Restore(int id);
    void Update(int id, RouteModel routeModel);
}