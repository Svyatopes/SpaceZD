namespace SpaceZD.BusinessLayer.Models;

public class PlatformMaintenanceModel
{
    public int Id { get; set; }
    public PlatformModel Platform { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PlatformMaintenanceModel model &&
               Id == model.Id &&
               EqualityComparer<PlatformModel>.Default.Equals(Platform, model.Platform) &&
               StartTime == model.StartTime &&
               EndTime == model.EndTime &&
               IsDeleted == model.IsDeleted;
    }
}