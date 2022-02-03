namespace SpaceZD.API.Models.Inputs
{
    public class TripStationInputModel
    {
        public virtual StationModel Station { get; set; }
        public virtual PlatformModel Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public virtual TripModel Trip { get; set; }
    }
}
