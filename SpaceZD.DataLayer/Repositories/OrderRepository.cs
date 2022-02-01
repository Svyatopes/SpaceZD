using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class OrderRepository
    {
        public Order GetOrderById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var order = context.Orders.Include(o => o.User)
                                      .Include(o => o.Trip)
                                      .Include(o => o.StartStation)
                                        .ThenInclude(s => s.Station)
                                      .Include(o => o.EndStation)
                                        .ThenInclude(s => s.Station)
                                      .FirstOrDefault(o => o.Id == id);
            return order;
        }

        public Order GetOrderByIdWithTickets(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var order = context.Orders.Include(o => o.User)
                                      .Include(o => o.Trip)
                                      .Include(o => o.StartStation)
                                        .ThenInclude(s => s.Station)
                                      .Include(o => o.EndStation)
                                        .ThenInclude(s => s.Station)
                                      .Include(o => o.Tickets)
                                      .FirstOrDefault(o => o.Id == id);
            return order;
        }

        public List<Order> GetOrders()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var orders = context.Orders.Include(o => o.User)
                                      .Include(o => o.Trip)
                                      .Include(o => o.StartStation)
                                        .ThenInclude(s => s.Station)
                                      .Include(o => o.EndStation)
                                        .ThenInclude(s => s.Station)
                                      .ToList();
            return orders;
        }

        public void AddOrder(Order order)
        {
            var context = VeryVeryImportantContext.GetInstance();
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public void EditOrder(Order order)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var orderInDB = GetOrderById(order.Id);

            if (orderInDB == null)
                throw new Exception($"Not found order with {order.Id} to edit");

            if (order.Trip != null && orderInDB.Trip.Id != order.Trip.Id)
                orderInDB.Trip = order.Trip;

            if (order.StartStation != null && orderInDB.StartStation.Id != order.StartStation.Id)
                orderInDB.StartStation = order.StartStation;

            if (order.EndStation != null && orderInDB.EndStation.Id != order.EndStation.Id)
                orderInDB.EndStation = order.EndStation;

            if (order.User != null && orderInDB.User.Id != order.User.Id)
                orderInDB.User = order.User;

            context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var orderInDB = context.Orders.FirstOrDefault(o => o.Id == id);

            if (orderInDB == null)
                throw new Exception($"Not found order with {id} to delete");

            orderInDB.IsDeleted = true;

            context.SaveChanges();
        }

        public void RestoreOrder(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var orderInDB = context.Orders.FirstOrDefault(o => o.Id == id);

            if (orderInDB == null)
                throw new Exception($"Not found order with {id} to restore");

            orderInDB.IsDeleted = false;

            context.SaveChanges();
        }

    }
}
