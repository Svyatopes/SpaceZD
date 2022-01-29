namespace SpaceZD.DataLayer.Entities;

public class Order
{
    public int Id { get; set; }
    public virtual User User { get; set; }
    public virtual Trip Trip { get; set; }
    public virtual TripStation StartStation { get; set; }
    public virtual TripStation EndStation { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
    public bool IsDeleted { get; set; }
}