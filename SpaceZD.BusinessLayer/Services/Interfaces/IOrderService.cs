using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IOrderService
    {
        int Add(int userId, OrderModel order);
        void Delete(int userId, int orderId);
        void Edit(int userId, int orderId, OrderModel order);
        OrderModel GetById(int userId, int orderId);
        List<OrderModel> GetList(int userId, int userOrdersId, bool allIncluded);
        void Restore(int userId, int orderId);
    }
}