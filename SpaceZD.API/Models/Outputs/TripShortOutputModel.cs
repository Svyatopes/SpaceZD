namespace SpaceZD.API.Models;

public class TripShortOutputModel
{
    public int Id { get; set; }
    public int TrainId { get; set; }
    public int RouteId { get; set; }
    public DateTime StartTime { get; set; }
}