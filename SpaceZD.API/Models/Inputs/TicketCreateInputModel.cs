
using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class TicketCreateInputModel : TicketUpdateInputModel
{
    [Required]
    public int OrderId { get; set; }  
    
    public bool IsTeaIncluded { get; set; }
    public bool IsPetPlaceIncluded { get; set; }

}