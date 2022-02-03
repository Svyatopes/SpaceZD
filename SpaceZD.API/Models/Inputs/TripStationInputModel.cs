namespace SpaceZD.API.Models
{
    public class TripStationInputModel
    {
        public int StationId { get; set; }
        public int PlatformId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public int TripId { get; set; }
    }
}
