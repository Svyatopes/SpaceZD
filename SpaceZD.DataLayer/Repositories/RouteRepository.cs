using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class RouteRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<Route>
{
    public RouteRepository(VeryVeryImportantContext context) : base(context) { }

    public Route? GetById(int id) =>
        _context.Routes
                .Include(r => r.Trips)
                .Include(r => r.StartStation)
                .Include(r => r.EndStation)
                .FirstOrDefault(r => r.Id == id);

    public List<Route> GetList(bool includeAll = false) => _context.Routes.Where(r => !r.IsDeleted || includeAll).ToList();

    public int Add(Route route)
    {
        _context.Routes.Add(route);
        _context.SaveChanges();
        return route.Id;
    }

    public void Update(Route routeOld, Route routeUpdate)
    {
        routeOld.Code = routeUpdate.Code;
        routeOld.StartTime = routeUpdate.StartTime;
        routeOld.StartStation = routeUpdate.StartStation;
        routeOld.EndStation = routeUpdate.EndStation;

        _context.SaveChanges();
    }

    public void Update(Route route, bool isDeleted)
    {
        route.IsDeleted = isDeleted;
        
        _context.SaveChanges();
    }
}