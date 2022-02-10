namespace SpaceZD.API.Models
{
    public class RouteTransitOutputModel
    {
        public int Id { get; set; }
        public TransitOutputModel Transit { get; set; }
        public TimeSpan DepartingTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
    }
}
