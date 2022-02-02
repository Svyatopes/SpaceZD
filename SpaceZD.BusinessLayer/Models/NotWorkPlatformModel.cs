namespace SpaceZD.BusinessLayer.Models;

public class NotWorkPlatformModel
{
    public int Id { get; set; }
    //public Platform Platform { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDeleted { get; set; }
}