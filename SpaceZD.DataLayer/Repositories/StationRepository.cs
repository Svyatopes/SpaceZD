﻿using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class StationRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<Station>
{
    public StationRepository(VeryVeryImportantContext context) : base(context) { }

    public Station? GetById(int id) =>
        _context.Stations
                .Include(s => s.Platforms)
                .FirstOrDefault(s => s.Id == id);

    public List<Station> GetList(bool includeAll = false) => _context.Stations.Where(s => !s.IsDeleted || includeAll).ToList();

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
}