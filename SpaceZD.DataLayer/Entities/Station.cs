namespace SpaceZD.DataLayer.Entities;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Platform> Platforms { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Route> RoutesWithStartStation { get; set; }
    public virtual ICollection<Route> RoutesWithEndStation { get; set; }
    public virtual ICollection<Transit> TransitsWithStartStation { get; set; }
    public virtual ICollection<Transit> TransitsWithEndStation { get; set; }
    public virtual ICollection<TripStation> TripStations { get; set; }

    private bool Equals(Station other)
    {
        return Name == other.Name && Platforms.SequenceEqual(other.Platforms) && IsDeleted == other.IsDeleted;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Station)obj);
    }
}