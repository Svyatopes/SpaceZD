namespace SpaceZD.API.Models;

public class TripStationUpdateInputModel
{
    public int PlatformId { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartingTime { get; set; }
}