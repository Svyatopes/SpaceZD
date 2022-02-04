namespace SpaceZD.BusinessLayer.Models
{
    public class TripStationModel
    {
        public int Id { get; set; }
        public virtual StationModel Station { get; set; }
        public virtual PlatformModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
    }
}
