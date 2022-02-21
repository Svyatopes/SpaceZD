namespace SpaceZD.API.Models;

public class TripCreateInputModel : TripUpdateInputModel
{
    public int RouteId { get; set; }
    public DateTime StartTime { get; set; }
}