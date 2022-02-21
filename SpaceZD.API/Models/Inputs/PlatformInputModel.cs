using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class PlatformInputModel
{
    [Required]
    [Range(0, 100)]
    public int Number { get; set; }

    [Required]
    public int StationId { get; set; }
}