using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface ITicketRepository : IRepositorySoftDelete<Ticket>
{
    List<Ticket> GetListById(int orderId);
    void UpdatePrice(Ticket ticketWithOldPrice, Ticket ticketWithNewPrice);
}
