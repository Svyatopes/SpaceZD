namespace SpaceZD.BusinessLayer.Models;

public class RouteModel
{
    public int Id { get; set; }
    public string Code { get; set; }
    public List<RouteTransitModel> Transits { get; set; }
    public DateTime StartTime { get; set; }
    public StationModel StartStation { get; set; }
    public StationModel EndStation { get; set; }
    public bool IsDeleted { get; set; }

    private bool Equals(RouteModel other)
    {
        return Code == other.Code &&
            StartTime.Hour == other.StartTime.Hour &&
            StartTime.Minute == other.StartTime.Minute &&
            StartTime.Second == other.StartTime.Second &&
            StartStation.Equals(other.StartStation) &&
            EndStation.Equals(other.EndStation) &&
            IsDeleted == other.IsDeleted;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((RouteModel)obj);
    }
}