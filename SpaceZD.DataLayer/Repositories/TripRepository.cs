using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.DataLayer.Repositories
{
    internal class TripRepository
    {

        private readonly VeryVeryImportantContext _context;
        public TripRepository() => _context = VeryVeryImportantContext.GetInstance();

        public void Add(Trip trip)
        {
            _context.Trips.Add(trip);
            Save();
        }

        public Trip GetEntity(int id)
        {
            var entity = _context.Trips.FirstOrDefault(c => c.Id == id);
            if (entity == null)
                throw new Exception($"Trip c Id = {id} не найден");

            return entity;
        }

        public IEnumerable<Trip> GetListEntity() => _context.Trips.ToList();

        public void Delete(int id)
        {
            GetEntity(id).IsDeleted = true;

            Save();
        }

        public void Update(Trip trip)
        {
            var entity = GetEntity(trip.Id);

            entity.Train = trip.Train;
            entity.Route = trip.Route;
            entity.Stations = trip.Stations;
            entity.StartTime = trip.StartTime;
            entity.Orders = trip.Orders;
            entity.IsDeleted = trip.IsDeleted;

            Save();
        }

        public void Save() => _context.SaveChanges();
    }
}
