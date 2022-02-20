namespace SpaceZD.BusinessLayer.Models;

public class CarriageModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public TrainModel Train { get; set; }
    public CarriageTypeModel Type { get; set; }
    public bool IsDeleted { get; set; }


    private bool Equals(CarriageModel other)
    {
        return Number == other.Number && Type.Equals(other.Type) && IsDeleted == other.IsDeleted;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((CarriageModel)obj);
    }
}