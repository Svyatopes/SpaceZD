using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class PlatformMaintenanceInputModel
{
    [Required]
    public int PlatformId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }
}