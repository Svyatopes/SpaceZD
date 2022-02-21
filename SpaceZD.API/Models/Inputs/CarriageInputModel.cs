using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class CarriageInputModel
{
    [Required]
    [Range(0, 100)]
    public int Number { get; set; }

    [Required]
    public int TypeId { get; set; }

    [Required]
    public int TrainId { get; set; }
}