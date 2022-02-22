
namespace SpaceZD.API.Models;

public class TicketCreateInputModel : TicketUpdateInputModel
{
    public int OrderId { get; set; }
    public bool IsTeaIncluded { get; set; }
    public bool IsPetPlaceIncluded { get; set; }

}