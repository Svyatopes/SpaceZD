using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.DataLayer.Repositories
{
    internal class TripStationRepository
    {
        private readonly VeryVeryImportantContext _context;
        public TripStationRepository() => _context = VeryVeryImportantContext.GetInstance();

        public void Add(TripStation tripStation)
        {
            _context.TripStations.Add(tripStation);
            Save();
        }

        public TripStation GetEntity(int id)
        {
            var entity = _context.TripStations.FirstOrDefault(c => c.Id == id);
            if (entity == null)
                throw new Exception($"TripStation c Id = {id} не найден");

            return entity;
        }

        public IEnumerable<TripStation> GetListEntity() => _context.TripStations.ToList();


        public void Update(TripStation tripStation)
        {
            var entity = GetEntity(tripStation.Id);

            entity.Station = tripStation.Station;
            entity.Platform = tripStation.Platform;
            entity.ArrivalTime = tripStation.ArrivalTime;
            entity.DepartingTime = tripStation.DepartingTime;
            entity.Trip = tripStation.Trip;
            entity.OrdersWithStartStation = tripStation.OrdersWithStartStation;
            entity.OrdersWithEndStation = tripStation.OrdersWithEndStation;

            Save();
        }

        public void Save() => _context.SaveChanges();
    }
}
