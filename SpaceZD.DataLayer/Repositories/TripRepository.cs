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

        public IEnumerable<Trip> GetAllDeleted() => _context.Trips.Where(t => t.IsDeleted).ToList();

        public bool Delete(int id)
        {
            var entity = GetById(id);

            if (entity == null) return false;

            entity.IsDeleted = true;
            Save();

            return true;
        }

        public bool Recover(int id)
        {
            var entity = GetById(id);

            if (entity == null) return false;

            entity.IsDeleted = false;
            Save();

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
            entity.Orders = trip.Orders;

            Save();
            return true;
        }

        private void Save() => _context.SaveChanges();
    }
}
