using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IOrderService : IDeleteRestoreUpdate<OrderModel>, IGetByIdWithUserId<OrderModel>, IAddWithUserId<OrderModel>
{
    List<OrderModel> GetList(int userId, int userOrdersId, bool allIncluded);
}