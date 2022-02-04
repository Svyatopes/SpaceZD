using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories
{
    public class OrderRepository : BaseRepository, IRepository<Order>, ISoftDelete<Order>
    {
        public Order GetById(int id) => _context.Orders.FirstOrDefault(o => o.Id == id);

        public List<Order> GetList(bool includeAll) => _context.Orders.Where(p => !p.IsDeleted || includeAll).ToList();

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public bool Update(Order order)
        {
            var orderInDB = GetById(order.Id);

            if (orderInDB == null)
                return false;

            orderInDB.Trip = order.Trip;
            orderInDB.StartStation = order.StartStation;
            orderInDB.EndStation = order.EndStation;

            _context.SaveChanges();

            return true;
        }

        public bool Update(int id, bool isDeleted)
        {
            var orderInDB = GetById(id);

            if (orderInDB == null)
                return false;

            orderInDB.IsDeleted = isDeleted;

            _context.SaveChanges();

            return true;
        }

    }
}
