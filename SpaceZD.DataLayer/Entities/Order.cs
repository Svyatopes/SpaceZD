using SpaceZD.DataLayer.Enums;

namespace SpaceZD.DataLayer.Entities;

public class Order
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public virtual User User { get; set; }
    public virtual Trip Trip { get; set; }
    public virtual TripStation StartStation { get; set; }
    public virtual TripStation EndStation { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Order order &&
               Status == order.Status &&
               User.Equals(order.User) &&
               Trip.Equals(order.Trip) &&
               StartStation.Equals(order.StartStation) &&
               EndStation.Equals(order.EndStation) &&
               Tickets.Equals(order.Tickets) &&
               IsDeleted == order.IsDeleted;
    }
}