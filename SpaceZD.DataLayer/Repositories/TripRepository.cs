﻿using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TripRepository
    {

        private readonly VeryVeryImportantContext _context;
        public TripRepository() => _context = VeryVeryImportantContext.GetInstance();

        public void Add(Trip trip)
        {
            _context.Trips.Add(trip);
            _context.SaveChanges();
        }

        public Trip? GetById(int id) => _context.Trips.FirstOrDefault(c => c.Id == id);

        public IEnumerable<Trip> GetList(bool includeAll = false) => _context.Trips.Where(t => !t.IsDeleted || includeAll).ToList();

        public bool Update(int id, bool isDeleted)
        {
            var entity = GetById(id);
            if (entity == null) return false;

            entity.IsDeleted = isDeleted;

            _context.SaveChanges();
            return true;
        }

        public bool Update(Trip trip)
        {
            var entity = GetById(trip.Id);
            if (entity == null) return false;

            entity.Train = trip.Train;
            entity.Route = trip.Route;
            entity.Stations = trip.Stations;
            entity.StartTime = trip.StartTime;

            _context.SaveChanges();
            return true;
        }
    }
}
