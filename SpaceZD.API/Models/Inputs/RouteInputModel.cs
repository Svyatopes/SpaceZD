using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class RouteInputModel
{
    [Required]
    [StringLength(100)]
    public string Code { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public int StartStationId { get; set; }

    [Required]
    public int EndStationId { get; set; }
}