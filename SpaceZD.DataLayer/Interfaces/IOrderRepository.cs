using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;
public interface IOrderRepository
{
    int Add(Order order);
    Order? GetById(int id);
    List<Order> GetList(int userId, bool includeAll = false);
    void Update(Order order, bool isDeleted);
    void Update(Order oldOrder, Order newOrder);
}