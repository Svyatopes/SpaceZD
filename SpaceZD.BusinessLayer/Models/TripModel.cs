namespace SpaceZD.BusinessLayer.Models
{
    public class TripModel
    {
        public int Id { get; set; }
        public virtual TrainModel Train { get; set; }
        public virtual RouteModel Route { get; set; }
        public virtual LinkedList<TripStationModel> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public virtual List<OrderModel> Orders { get; set; }
        public bool IsDeleted { get; set; }
    }
}
