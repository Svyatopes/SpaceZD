namespace SpaceZD.API.Models;

public class RouteTransitInputModel
{
    public int TransitId { get; set; }
    public TimeSpan DepartingTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
}