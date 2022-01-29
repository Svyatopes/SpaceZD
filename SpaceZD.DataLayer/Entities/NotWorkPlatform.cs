namespace SpaceZD.DataLayer.Entities;

public class NotWorkPlatform
{
    public int Id { get; set; }
    public virtual Platform Platform { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDeleted { get; set; }
}