using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITicketService : IDeleteRestoreUpdate<TicketModel>, IGetByIdWithUserId<TicketModel>, IAddWithUserId<TicketModel>
{
    List<TicketModel> GetListByOrderId(int orderId, int userId);
    List<TicketModel> GetList(int userId);
    List<TicketModel> GetListDeleted(int userId);
}