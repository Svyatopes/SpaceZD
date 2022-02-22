namespace SpaceZD.BusinessLayer.Models;

public class CarriageTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public double PriceFactor { get; set; }
    public bool IsDeleted { get; set; }


    private bool Equals(CarriageTypeModel other)
    {
        return Name == other.Name &&
            NumberOfSeats == other.NumberOfSeats &&
            Math.Abs(PriceFactor - other.PriceFactor) < 0.001 &&
            IsDeleted == other.IsDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((CarriageTypeModel)obj);
    }
}