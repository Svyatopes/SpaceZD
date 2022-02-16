using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class StationRepository : BaseRepository, IStationRepository
{
    public StationRepository(VeryVeryImportantContext context) : base(context) {}

    public Station? GetById(int id)
    {
        var entity = _context.Stations
                             .Include(s => s.Platforms.Where(pl => !pl.IsDeleted))
                             .FirstOrDefault(s => s.Id == id);
        if (entity is null)
            return null;
        entity.Platforms = entity.Platforms.Where(pl => !pl.IsDeleted).ToList();
        return entity;
    }

    public List<Station> GetList(bool includeAll = false)
    {
        var entities = _context.Stations.Include(s => s.Platforms.Where(pl => !pl.IsDeleted)).Where(s => !s.IsDeleted || includeAll).ToList();
        foreach (var station in entities)
            station.Platforms = station.Platforms.Where(pl => !pl.IsDeleted).ToList();

        return entities;
    }

    public int Add(Station station)
    {
        _context.Stations.Add(station);
        _context.SaveChanges();
        return station.Id;
    }

    public void Update(Station stationOld, Station stationUpdate)
    {
        stationOld.Name = stationUpdate.Name;

        _context.SaveChanges();
    }

    public void Update(Station station, bool isDeleted)
    {
        station.IsDeleted = isDeleted;

        _context.SaveChanges();
    }

    public List<Platform> GetReadyPlatformsStation(Station station, DateTime moment)
    {
        return station.Platforms
                      .Where(pl => !pl.IsDeleted &&
                           !pl.PlatformMaintenances
                              .Where(t => !t.IsDeleted)
                              .Any(pm => pm.StartTime <= moment && pm.EndTime >= moment) &&
                           !pl.TripStations
                              .Any(ts => ts.ArrivalTime <= moment && ts.DepartingTime >= moment))
                      .ToList();
    }

    public List<Station> GetNearStations(Station station)
    {
        return station.TransitsWithStartStation
                      .Where(t => !t.IsDeleted)
                      .Select(t => t.EndStation)
                      .Where(t => !t.IsDeleted)
                      .ToList();
    }
}