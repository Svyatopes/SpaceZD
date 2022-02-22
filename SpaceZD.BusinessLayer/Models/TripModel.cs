namespace SpaceZD.BusinessLayer.Models
{
    public class TripModel
    {
        public int Id { get; set; }
        public TrainModel Train { get; set; }
        public RouteModel Route { get; set; }
        public List<TripStationModel> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public List<OrderModel> Orders { get; set; }
        public bool IsDeleted { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TripModel model &&
                   Train.Equals(model.Train) &&
                   Route.Equals(model.Route) &&
                   StartTime == model.StartTime &&
                   IsDeleted == model.IsDeleted;
        }
    }
}
