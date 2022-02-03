﻿using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TripStationRepository
    {
        private readonly VeryVeryImportantContext _context;
        public TripStationRepository() => _context = VeryVeryImportantContext.GetInstance();

        public void Add(TripStation tripStation)
        {
            _context.TripStations.Add(tripStation);
            Save();
        }

        public TripStation? GetById(int id) => _context.TripStations.FirstOrDefault(c => c.Id == id);

        public IEnumerable<TripStation> GetAll() => _context.TripStations.ToList();

        public bool Update(TripStation tripStation)
        {
            var entity = GetById(tripStation.Id);
            if (entity == null) return false;

            entity.Platform = tripStation.Platform;
            entity.ArrivalTime = tripStation.ArrivalTime;
            entity.DepartingTime = tripStation.DepartingTime;

            Save();
            return true;
        }

        private void Save() => _context.SaveChanges();
    }
}
