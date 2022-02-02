using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

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

        public TripStation? GetEntity(int id) => _context.TripStations.FirstOrDefault(c => c.Id == id);

        public IEnumerable<TripStation> GetListEntity() => _context.TripStations.ToList();

        public bool Update(TripStation tripStation)
        {
            var entity = GetEntity(tripStation.Id);
            if (entity == null) return false;

            entity.Station = tripStation.Station;
            entity.Platform = tripStation.Platform;
            entity.ArrivalTime = tripStation.ArrivalTime;
            entity.DepartingTime = tripStation.DepartingTime;
            entity.Trip = tripStation.Trip;
            entity.OrdersWithStartStation = tripStation.OrdersWithStartStation;
            entity.OrdersWithEndStation = tripStation.OrdersWithEndStation;

            Save();
            return true;
        }

        private void Save() => _context.SaveChanges();
    }
}
