using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceZD.DataLayer.Entities;

[NotMapped]
public class CarriageSeats : ICloneable
{
    public Carriage Carriage { get; set; }
    public List<Seat> Seats { get; set; }


    private bool Equals(CarriageSeats other)
    {
        return Carriage.Equals(other.Carriage) && Seats.SequenceEqual(other.Seats);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((CarriageSeats)obj);
    }

    public object Clone()
    {
        var clone = new CarriageSeats { Carriage = Carriage, Seats = new List<Seat>() };
        foreach (var seat in Seats)
            clone.Seats.Add(new Seat { NumberOfSeats = seat.NumberOfSeats, IsFree = seat.IsFree });
        
        return clone;
    }
}