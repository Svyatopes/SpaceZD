using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITicketService
    {
        int Add(TicketModel entity, int userId);
        void Delete(int id, int userId);
        TicketModel GetById(int id, int userId);
        List<TicketModel> GetListByOrderId(int orderId, int userId);
        List<TicketModel> GetList(int userId);
        List<TicketModel> GetListDeleted(int userId);
        void Restore(int id, int userId);
        void Update(int id, TicketModel entity, int userId);
    }
}