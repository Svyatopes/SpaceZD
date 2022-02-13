namespace SpaceZD.API.Models;

public class TicketInputModel
{
    public int OrderId { get; set; }
    public int CarriageId { get; set; }
    public int SeatNumber { get; set; }
    public bool IsTeaIncluded { get; set; }
    public bool IsPetPlaceIncluded { get; set; }
    public int PersonId { get; set; }
}