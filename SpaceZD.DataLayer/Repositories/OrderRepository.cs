using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class OrderRepository : BaseRepository, IRepositorySoftDelete<Order>
{
    public OrderRepository(VeryVeryImportantContext context) : base(context) { }

    public Order? GetById(int id)
    {
        var order = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Trip)
                .Include(o => o.StartStation)
                .Include(o => o.EndStation)
                .Include(o => o.Tickets.Where(t => !t.IsDeleted))
                .FirstOrDefault(o => o.Id == id);
        if (order == null)
            return null;
        order.Tickets = order.Tickets.Where(t => !t.IsDeleted).ToList();
        return order;
    }

    public List<Order> GetList(bool includeAll = false) =>
        _context.Orders
        .Include(o => o.User)
        .Include(o => o.Trip)
        .Include(o => o.StartStation)
        .Include(o => o.EndStation)
        .Where(p => !p.IsDeleted || includeAll)
        .ToList();



    public int Add(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        return order.Id;
    }

    public void Update(Order oldOrder, Order newOrder)
    {
        oldOrder.Trip = newOrder.Trip;
        oldOrder.StartStation = newOrder.StartStation;
        oldOrder.EndStation = newOrder.EndStation;
        _context.SaveChanges();
    }

    public void Update(Order order, bool isDeleted)
    {
        order.IsDeleted = isDeleted;
        _context.SaveChanges();
    }
}