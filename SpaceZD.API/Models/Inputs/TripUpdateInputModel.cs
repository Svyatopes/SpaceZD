using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class TripUpdateInputModel
{
    [Required]
    public int TrainId { get; set; }
}