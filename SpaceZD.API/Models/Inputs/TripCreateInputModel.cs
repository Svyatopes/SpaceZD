using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class TripCreateInputModel : TripUpdateInputModel
{
    [Required]
    public int RouteId { get; set; }

    [Required]
    public DateTime StartTime { get; set; }
}