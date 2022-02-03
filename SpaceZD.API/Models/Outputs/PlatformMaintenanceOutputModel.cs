namespace SpaceZD.API.Models;

public class PlatformMaintenanceOutputModel
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}