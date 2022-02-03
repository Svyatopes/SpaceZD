namespace SpaceZD.API.Models
{
    public class TripOutputModel
    {
        public int Id { get; set; }
        public TrainOutputModel Train { get; set; }
        public RouteOutputModel Route { get; set; }
        public ICollection<TripStationOutputModel> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public ICollection<OrderOutputModel> Orders { get; set; }
        public bool IsDeleted { get; set; }
    }
}
