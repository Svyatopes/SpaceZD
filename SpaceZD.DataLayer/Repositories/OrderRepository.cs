using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class OrderRepository : BaseRepository, IRepositorySoftDelete<Order>
{
    public OrderRepository(VeryVeryImportantContext context) : base(context) { }

    public Order? GetById(int id) =>
        _context.Orders
                .Include(o => o.User)
                .Include(o => o.Trip)
                .Include(o => o.StartStation)
                .Include(o => o.EndStation)
                .Include(o => o.Tickets)
                .FirstOrDefault(o => o.Id == id);

    public IEnumerable<Order> GetList(bool includeAll = false) => _context.Orders.Where(p => !p.IsDeleted || includeAll).ToList();

    public int Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return order.Id;
    }

    public bool Update(Order order)
    {
        var orderInDb = GetById(order.Id);

        if (orderInDb == null)
            return false;

        orderInDb.Trip = order.Trip;
        orderInDb.StartStation = order.StartStation;
        orderInDb.EndStation = order.EndStation;

        _context.SaveChanges();

        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var orderInDb = GetById(id);

        if (orderInDb == null)
            return false;

        orderInDb.IsDeleted = isDeleted;

        _context.SaveChanges();

        return true;
    }
}