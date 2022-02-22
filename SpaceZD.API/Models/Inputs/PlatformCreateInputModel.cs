using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models
{
    public class PlatformCreateInputModel : PlatformUpdateInputModel
    {
        [Required]
        public int StationId { get; set; }
    }
}
