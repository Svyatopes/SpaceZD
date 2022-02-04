namespace SpaceZD.DataLayer.Entities;

public class Platform
{
    public int Id { get; set; }
    public int Number { get; set; }
    public virtual Station Station { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<PlatformMaintenance> PlatformMaintenances { get; set; }
    public virtual ICollection<TripStation> TripStations { get; set; }
}