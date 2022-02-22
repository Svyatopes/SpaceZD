namespace SpaceZD.DataLayer.Entities;

public class RouteTransit
{
    public int Id { get; set; }
    public virtual Transit Transit { get; set; }
    public TimeSpan DepartingTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public bool IsDeleted { get; set; }
    public virtual Route Route { get; set; }

    private bool Equals(RouteTransit other)
    {
        return DepartingTime.Equals(other.DepartingTime) &&
               ArrivalTime.Equals(other.ArrivalTime) &&
               IsDeleted == other.IsDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((RouteTransit)obj);
    }
}