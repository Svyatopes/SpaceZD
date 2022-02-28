using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class RouteTransitRepository : BaseRepository, IRouteTransitRepository
{
    public RouteTransitRepository(VeryVeryImportantContext context) : base(context) { }

    public RouteTransit? GetById(int id) =>
        _context.RouteTransits
                .Include(rt => rt.Transit)
                .FirstOrDefault(rt => rt.Id == id);

    public List<RouteTransit> GetList(int routeId, bool includeAll = false) =>
        _context.RouteTransits
        .Where(r => (!r.IsDeleted || includeAll) && r.Route.Id == routeId)
        .ToList();

    public int Add(RouteTransit routeTransit)
    {
        _context.RouteTransits.Add(routeTransit);
        _context.SaveChanges();
        return routeTransit.Id;
    }

    public void Update(RouteTransit oldRouteTransit, RouteTransit newRouteTransit)
    {
        oldRouteTransit.Transit = newRouteTransit.Transit;
        oldRouteTransit.DepartingTime = newRouteTransit.DepartingTime;
        oldRouteTransit.ArrivalTime = newRouteTransit.ArrivalTime;
        oldRouteTransit.Route = newRouteTransit.Route;

        _context.SaveChanges();
    }

    public void Update(RouteTransit routeTransit, bool isDeleted)
    {
        routeTransit.IsDeleted = isDeleted;
        _context.SaveChanges();
    }
}