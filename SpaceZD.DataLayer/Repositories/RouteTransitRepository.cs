using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class RouteTransitRepository : BaseRepository, IRepositorySoftDelete<RouteTransit>
{
    public RouteTransitRepository(VeryVeryImportantContext context) : base(context) { }

    public RouteTransit? GetById(int id) => _context.RouteTransits.FirstOrDefault(rt => rt.Id == id);

    public IEnumerable<RouteTransit> GetList(bool includeAll = false) => _context.RouteTransits.Where(r => !r.IsDeleted || includeAll).ToList();

    public void Add(RouteTransit routeTransit)
    {
        _context.RouteTransits.Add(routeTransit);
        _context.SaveChanges();
    }

    public bool Update(RouteTransit routeTransit)
    {
        var entity = GetById(routeTransit.Id);
        if (entity is null)
            return false;

        entity.Transit = routeTransit.Transit;
        entity.DepartingTime = routeTransit.DepartingTime;
        entity.ArrivalTime = routeTransit.ArrivalTime;
        entity.Route = routeTransit.Route;

        _context.SaveChanges();
        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var entity = GetById(id);
        if (entity is null)
            return false;

        entity.IsDeleted = isDeleted;
        _context.SaveChanges();
        return true;
    }
}