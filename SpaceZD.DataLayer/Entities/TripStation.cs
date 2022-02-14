namespace SpaceZD.DataLayer.Entities;

public class TripStation
{
    public int Id { get; set; }
    public virtual Station Station { get; set; }
    public virtual Platform Platform { get; set; }
    public DateTime ArrivalTime { get; set; }
    public DateTime DepartingTime { get; set; }
    public virtual Trip Trip { get; set; }
    public virtual ICollection<Order> OrdersWithStartStation { get; set; }
    public virtual ICollection<Order> OrdersWithEndStation { get; set; }


    private bool Equals(TripStation other)
    {
        return Station.Equals(other.Station) && Platform.Equals(other.Platform) && 
            ArrivalTime == other.ArrivalTime && DepartingTime == other.DepartingTime && Trip.Id == other.Trip.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((TripStation)obj);
    }
}