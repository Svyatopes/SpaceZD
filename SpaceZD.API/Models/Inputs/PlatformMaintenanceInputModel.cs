namespace SpaceZD.API.Models;

public class PlatformMaintenanceInputModel
{
    public int PlatformId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}