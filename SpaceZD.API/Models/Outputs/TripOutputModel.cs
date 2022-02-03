namespace SpaceZD.API.Models.Outputs
{
    public class TripOutputModel
    {
        public int Id { get; set; }
        public TrainModel Train { get; set; }
        public RouteModel Route { get; set; }
        public ICollection<TripStationModel> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public ICollection<OrderModel> Orders { get; set; }
        public bool IsDeleted { get; set; }
    }
}
