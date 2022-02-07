namespace SpaceZD.DataLayer.Entities;

public class Route
{
    public int Id { get; set; }
    public string Code { get; set; }
    public virtual ICollection<RouteTransit> Transits { get; set; }
    public DateTime StartTime { get; set; }
    public virtual Station StartStation { get; set; }
    public virtual Station EndStation { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Trip> Trips { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Route route)
            return Code == route.Code &&
                   StartTime.Equals(route.StartTime) &&
                   StartStation == route.StartStation &&
                   EndStation == route.EndStation &&
                   IsDeleted == route.IsDeleted &&
                   Transits.SequenceEqual(route.Transits);
        
        return false;
    }
}