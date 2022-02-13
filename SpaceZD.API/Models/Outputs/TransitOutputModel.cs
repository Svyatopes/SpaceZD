namespace SpaceZD.API.Models;

public class TransitOutputModel
{
    public int Id { get; set; }
    public int StartStationId { get; set; }
    public int EndStationId { get; set; }
    public decimal? Price { get; set; }
}