using SpaceZD.DataLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class OrderAddInputModel
{
    [Required]
    public int TripId { get; set; }
    
    [Required]
    public int StartTripStationId { get; set; }
    
    [Required]
    public int EndTripStationId { get; set; }
}