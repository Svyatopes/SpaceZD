namespace SpaceZD.BusinessLayer.Models;

public class SeatModel
{
    public int NumberOfSeats { get; set; }
    public bool IsFree { get; set; }


    private bool Equals(SeatModel other)
    {
        return NumberOfSeats == other.NumberOfSeats && IsFree == other.IsFree;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((SeatModel)obj);
    }
}