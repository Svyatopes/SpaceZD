namespace SpaceZD.API.Models;

public class PlatformMaintenanceInsertInputModel
{
    public int PlatformId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}