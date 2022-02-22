using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class CarriageTypeInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(1, 1000)]
    public int NumberOfSeats { get; set; }
    
    [Required]
    [Range(0.001, 999)]
    public double PriceFactor { get; set; }
}