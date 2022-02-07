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

    public override bool Equals(object? obj)
    {
        if (obj is Station station)
            return Name == station.Name &&
                IsDeleted == station.IsDeleted &&
                Platforms.SequenceEqual(station.Platforms);

        return false;
    }
}