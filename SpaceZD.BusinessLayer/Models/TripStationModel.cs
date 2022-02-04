namespace SpaceZD.BusinessLayer.Models
{
    public class TripStationModel
    {
        public int Id { get; set; }
        public StationModel Station { get; set; }
        public PlatformModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
    }
}
