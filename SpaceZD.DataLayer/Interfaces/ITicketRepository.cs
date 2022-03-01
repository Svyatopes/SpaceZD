using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface ITicketRepository : IRepositorySoftDelete<Ticket>
{
    List<Ticket> GetListByOrderId(int orderId);
    
}
