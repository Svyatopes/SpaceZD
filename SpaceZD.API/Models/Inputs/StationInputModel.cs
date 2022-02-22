using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class StationInputModel
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}