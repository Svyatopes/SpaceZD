namespace SpaceZD.DataLayer.Entities;

public class Transit
{
    public int Id { get; set; }
    public virtual Station StartStation { get; set; }
    public virtual Station EndStation { get; set; }
    public decimal? Price { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<RouteTransit> RouteTransit { get; set; }

    private bool Equals(Transit other)
    {
        return StartStation.Equals(other.StartStation) &&
            EndStation.Equals(other.EndStation) &&
            Price == other.Price &&
            IsDeleted == other.IsDeleted;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Transit)obj);
    }
}