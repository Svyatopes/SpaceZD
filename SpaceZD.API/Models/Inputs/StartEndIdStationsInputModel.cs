using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class StartEndIdStationsInputModel
{
    [Required]
    public int StartStationId { get; set; }

    [Required]
    public int EndStationId { get; set; }
}