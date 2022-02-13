namespace SpaceZD.API.Models;

public class OrderFullOutputModel : OrderShortOutputModel
{
    public List<TicketOutputModel> Tickets { get; set; }
}