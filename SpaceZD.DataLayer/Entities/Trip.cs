namespace SpaceZD.DataLayer.Entities;

public class Trip
{
    public int Id { get; set; }
    public virtual Train Train { get; set; }
    public virtual Route Route { get; set; }
    public virtual ICollection<TripStation> Stations { get; set; }
    public DateTime StartTime { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public bool IsDeleted { get; set; }


    private bool Equals(Trip other)
    {
        return Train.Equals(other.Train) &&
            Route.Equals(other.Route) &&
            Stations.SequenceEqual(other.Stations) &&
            StartTime.Equals(other.StartTime) &&
            IsDeleted == other.IsDeleted;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Trip)obj);
    }
}