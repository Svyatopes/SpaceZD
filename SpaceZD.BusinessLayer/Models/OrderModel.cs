using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Models;

public class OrderModel
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; }
    public UserModel User { get; set; }
    public TripModel Trip { get; set; }
    public TripStationModel StartStation { get; set; }
    public TripStationModel EndStation { get; set; }
    public List<TicketModel> Tickets { get; set; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is OrderModel model &&
               Status == model.Status &&
               User.Equals(model.User) &&
               Trip.Equals(model.Trip) &&
               StartStation.Equals(model.StartStation) &&
               EndStation.Equals(model.EndStation) &&
               Tickets.SequenceEqual(model.Tickets) &&
               IsDeleted == model.IsDeleted;
    }
}
