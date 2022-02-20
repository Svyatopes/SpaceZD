namespace SpaceZD.DataLayer.Entities;

public class Train
{
    public int Id { get; set; }
    public virtual ICollection<Carriage> Carriages { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Trip> Trips { get; set; }

    private bool Equals(Train other)
    {
        return Carriages.SequenceEqual(other.Carriages) && IsDeleted == other.IsDeleted;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Train)obj);
    }
}