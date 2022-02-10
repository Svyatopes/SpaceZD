namespace SpaceZD.API.Models
{
    public class TripStationOutputModel
    {
        public int Id { get; set; }
        public int StationId { get; set; }
        public int PlatformId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
    }
}
