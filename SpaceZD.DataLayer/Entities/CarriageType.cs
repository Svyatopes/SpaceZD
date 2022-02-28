namespace SpaceZD.DataLayer.Entities;

public class CarriageType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal PriceCoefficient { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Carriage> Carriages { get; set; }
    
    
    private bool Equals(CarriageType other)
    {
        return Name == other.Name &&
            NumberOfSeats == other.NumberOfSeats &&
            PriceCoefficient == other.PriceCoefficient &&
            IsDeleted == other.IsDeleted;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((CarriageType)obj);
    }
}