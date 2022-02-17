namespace SpaceZD.DataLayer.Entities;

public class Carriage
{
    public int Id { get; set; }
    public int Number { get; set; }
    public virtual CarriageType Type { get; set; }
    public bool IsDeleted { get; set; }
    public virtual Train Train { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }


    private bool Equals(Carriage other)
    {
        return Number == other.Number && 
            Type.Equals(other.Type) && 
            IsDeleted == other.IsDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Carriage)obj);
    }
}