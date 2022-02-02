using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

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

        public Trip? GetById(int id) => _context.Trips.FirstOrDefault(c => c.Id == id);

        public IEnumerable<Trip> GetAll() => _context.Trips.Where(t => !t.IsDeleted).ToList();

        public bool Update(Trip trip, bool IsDeleted)
        {
            var entity = GetById(trip.Id);
            if (entity == null) return false;

            entity.Train = trip.Train;
            entity.Route = trip.Route;
            entity.Stations = trip.Stations;
            entity.StartTime = trip.StartTime;
            entity.Orders = trip.Orders;
            entity.IsDeleted = IsDeleted;

            Save();
            return true;
        }

        private void Save() => _context.SaveChanges();
    }
}
