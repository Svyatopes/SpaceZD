namespace SpaceZD.API.Models;

public class RouteOutputModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime StartTime { get; set; }
    public int StartStationId { get; set; }
    public int EndStationId { get; set; }
}