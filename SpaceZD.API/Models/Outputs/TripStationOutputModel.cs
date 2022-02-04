namespace SpaceZD.API.Models
{
    public class TripStationOutputModel
    {
        public int Id { get; set; }
        public StationShortOutputModel Station { get; set; }
        public PlatformOutputModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public TripShortOutputModel Trip { get; set; }
    }
}
