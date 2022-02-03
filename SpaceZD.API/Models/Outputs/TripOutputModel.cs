namespace SpaceZD.API.Models.Outputs
{
    public class TripOutputModel
    {
        public int Id { get; set; }
        public virtual TrainModel Train { get; set; }
        public virtual RouteModel Route { get; set; }
        public virtual ICollection<TripStationModel> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public virtual ICollection<OrderModel> Orders { get; set; }
        public bool IsDeleted { get; set; }
    }
}
