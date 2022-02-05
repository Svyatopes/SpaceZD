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
    }
}
