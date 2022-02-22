namespace SpaceZD.DataLayer.Entities;

public class Platform
{
    public int Id { get; set; }
    public int Number { get; set; }
    public virtual Station Station { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<PlatformMaintenance> PlatformMaintenances { get; set; }
    public virtual ICollection<TripStation> TripStations { get; set; }
    public int StationId { get; set; }

    private bool Equals(Platform other)
    {
        return Number == other.Number && 
            Station.Name == other.Station.Name && 
            IsDeleted == other.IsDeleted;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Platform)obj);
    }
}