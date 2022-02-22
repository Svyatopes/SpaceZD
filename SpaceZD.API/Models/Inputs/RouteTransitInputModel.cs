using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class RouteTransitInputModel
{
    [Required]
    public int TransitId { get; set; }

    [Required]
    public TimeSpan DepartingTime { get; set; }

    [Required]
    public TimeSpan ArrivalTime { get; set; }

    [Required]
    public int RouteId { get; set; }
}