namespace SpaceZD.API.Models;

public class RouteTransitOutputModel
{
    public int Id { get; set; }
    public int TransitId { get; set; }
    public TimeSpan DepartingTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public int RouteId { get; set; }
}