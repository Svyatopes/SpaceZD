using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITicketService : IGetByIdWithUserId<TicketModel>, IAddWithUserId<TicketModel>
{
    List<TicketModel> GetList(int userId);
    List<TicketModel> GetListDeleted(int userId);
    void Delete(int userId, int id);
    void Restore(int userId, int id);
}