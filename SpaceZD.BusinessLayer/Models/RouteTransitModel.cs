namespace SpaceZD.BusinessLayer.Models
{
    public class RouteTransitModel
    {
        public int Id { get; set; }
        public TransitModel Transit { get; set; }
        public TimeSpan DepartingTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
