using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class PlatformUpdateInputModel
{
    [Required]
    [Range(0, 100)]
    public int Number { get; set; }
}