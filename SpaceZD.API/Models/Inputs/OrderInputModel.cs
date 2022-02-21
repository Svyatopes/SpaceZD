namespace SpaceZD.API.Models;

public class OrderInputModel
{
    public int TripId { get; set; }
    public int StartTripStationId { get; set; }
    public int EndTripStationId { get; set; }
}