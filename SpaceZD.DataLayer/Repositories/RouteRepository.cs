using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class RouteRepository : BaseRepository, IRepositorySoftDelete<Route>
{
    public RouteRepository(VeryVeryImportantContext context) : base(context) { }

    public Route? GetById(int id) =>
        _context.Routes
                .Include(r => r.Trips)
                .Include(r => r.StartStation)
                .Include(r => r.EndStation)
                .FirstOrDefault(r => r.Id == id);

    public IEnumerable<Route> GetList(bool includeAll = false) => _context.Routes.Where(r => !r.IsDeleted || includeAll).ToList();

    public int Add(Route route)
    {
        _context.Routes.Add(route);
        _context.SaveChanges();
        return route.Id;
    }

    public bool Update(Route route)
    {
        var entity = GetById(route.Id);
        if (entity is null)
            return false;

        entity.Code = route.Code;
        entity.StartTime = route.StartTime;
        entity.StartStation = route.StartStation;
        entity.EndStation = route.EndStation;

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