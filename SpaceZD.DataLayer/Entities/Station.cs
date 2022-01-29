namespace SpaceZD.DataLayer.Entities;

public class Station
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Station> NearStation { get; set; }
    public virtual ICollection<Platform> Platforms { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Route> Routes { get; set; }
    public virtual ICollection<TripStation> TripStations { get; set; }
    public virtual ICollection<Transit> Transits { get; set; }
}