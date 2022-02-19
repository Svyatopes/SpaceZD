namespace SpaceZD.DataLayer.Entities;

public class PlatformMaintenance
{
    public int Id { get; set; }
    public virtual Platform Platform { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PlatformMaintenance maintenance &&
               Id == maintenance.Id &&
               EqualityComparer<Platform>.Default.Equals(Platform, maintenance.Platform) &&
               StartTime == maintenance.StartTime &&
               EndTime == maintenance.EndTime &&
               IsDeleted == maintenance.IsDeleted;
    }
}