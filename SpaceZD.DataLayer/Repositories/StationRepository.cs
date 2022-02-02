using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class StationRepository
    {
        public Station GetStationById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var station = context.Stations.FirstOrDefault(s => s.Id == id);

            return station;
        }

        public List<Station> GetStations()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var stations = context.Stations.ToList();

            return stations;
        }

        public void AddStation(Station station)
        {
            var context = VeryVeryImportantContext.GetInstance();

            context.Stations.Add(station);

            context.SaveChanges();
        }

        public void DeleteStation(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var station = context.Stations.FirstOrDefault(s => s.Id == id);

            station.IsDeleted = true;

            context.SaveChanges();
        }

        public void UpdateStation(Station station)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var stationDB = GetStationById(station.Id);

            stationDB.Name = station.Name;
            stationDB.Platforms = station.Platforms;
            stationDB.IsDeleted=station.IsDeleted;
            stationDB.RoutesWithStartStation = station.RoutesWithStartStation;
            stationDB.RoutesWithEndStation = station.RoutesWithEndStation;
            stationDB.TransitsWithStartStation = station.TransitsWithStartStation;
            stationDB.TransitsWithEndStation=station.TransitsWithEndStation;
            stationDB.TripStations = station.TripStations;

            context.SaveChanges();
        }
    }
}
