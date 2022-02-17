using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceZD.DataLayer.Entities;

[NotMapped]
public class Seat
{
    public int NumberOfSeats { get; set; }
    public bool IsFree { get; set; }


    private bool Equals(Seat other)
    {
        return NumberOfSeats == other.NumberOfSeats && IsFree == other.IsFree;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Seat)obj);
    }
}