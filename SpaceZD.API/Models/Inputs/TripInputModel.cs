namespace SpaceZD.API.Models.Inputs
{
    public class TripInputModel
    {
        public virtual TrainModel Train { get; set; }
        public virtual RouteModel Route { get; set; }
        public virtual ICollection<TripStationModel> Stations { get; set; }
        public DateTime StartTime { get; set; }
    }
}
