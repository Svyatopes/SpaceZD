namespace SpaceZD.API.Models.Outputs
{
    public class TripStationOutputModel
    {
        public int Id { get; set; }
        public StationOutputModel Station { get; set; }
        public PlatformOutputModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public TripOutputModel Trip { get; set; }
        public ICollection<OrderOutputModel> OrdersWithStartStation { get; set; }
        public ICollection<OrderOutputModel> OrdersWithEndStation { get; set; }
    }
}
