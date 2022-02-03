namespace SpaceZD.API.Models
{
    public class TripStationUpdateModel
    {
        public int PlatformId { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
    }
}
