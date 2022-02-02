namespace SpaceZD.API.Models;

public class PlatformMaintenanceCreateInputModel
{
    public int PlatformId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}