using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class TripStationUpdateInputModel
{
    [Required]
    public int PlatformId { get; set; }

    [Required]
    public DateTime ArrivalTime { get; set; }

    [Required]
    public DateTime DepartingTime { get; set; }
}