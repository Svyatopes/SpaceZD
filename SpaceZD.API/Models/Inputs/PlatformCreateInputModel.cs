using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models
{
    public class PlatformCreateInputModel : PlatformInputModel
    {
        [Required]
        public int StationId { get; set; }
    }
}
