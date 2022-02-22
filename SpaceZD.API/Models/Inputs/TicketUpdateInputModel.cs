using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models
{
    public class TicketUpdateInputModel
    {
        [Required]
        public int CarriageId { get; set; }

        [Required]
        [Range(1, 1000)]
        public int SeatNumber { get; set; }

        [Required]
        public int PersonId { get; set; }
    }
}
