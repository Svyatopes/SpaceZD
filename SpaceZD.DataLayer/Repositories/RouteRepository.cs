using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class RouteRepository : BaseRepository, IRepositorySoftDelete<Route>
{
    public RouteRepository(VeryVeryImportantContext context) : base(context) {}

    public Route? GetById(int id)
    {
        var entity = _context.Routes
                             .Include(r => r.RouteTransits.Where(t => !t.IsDeleted).OrderBy(g => g.ArrivalTime))
                             .Include(r => r.StartStation)
                             .Include(r => r.EndStation)
                             .FirstOrDefault(r => r.Id == id);
        if (entity is null)
            return null;
        entity.RouteTransits = entity.RouteTransits
                                .Where(t => !t.IsDeleted)
                                .OrderBy(g => g.ArrivalTime)
                                .ToList();

        return entity;
    }

    public List<Route> GetList(bool includeAll = false)
    {
        var entities = _context.Routes
                               .Include(r => r.RouteTransits.Where(t => !t.IsDeleted).OrderBy(g => g.ArrivalTime))
                               .Include(r => r.StartStation)
                               .Include(r => r.EndStation)
                               .Where(r => !r.IsDeleted || includeAll).ToList();
        foreach (var route in entities)
            route.RouteTransits = route.RouteTransits
                                  .Where(t => !t.IsDeleted)
                                  .OrderBy(g => g.ArrivalTime)
                                  .ToList();

        return entities;
    }

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