namespace SpaceZD.API.Models.Outputs
{
    public class TripStationOutputModel
    {
        public int Id { get; set; }
        public StationModel Station { get; set; }
        public PlatformModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public TripModel Trip { get; set; }
        public ICollection<OrderModel> OrdersWithStartStation { get; set; }
        public ICollection<OrderModel> OrdersWithEndStation { get; set; }
    }
}
