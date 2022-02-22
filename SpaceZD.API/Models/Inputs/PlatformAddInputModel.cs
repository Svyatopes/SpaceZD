using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models
{
    public class PlatformAddInputModel : PlatformInputModel
    {
        [Required]
        public int StationId { get; set; }
    }
}
