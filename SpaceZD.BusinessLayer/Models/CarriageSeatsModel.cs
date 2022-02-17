namespace SpaceZD.BusinessLayer.Models;

public class CarriageSeatsModel : ICloneable
{
    public CarriageModel Carriage { get; set; }
    public List<SeatModel> Seats { get; set; }


    private bool Equals(CarriageSeatsModel other)
    {
        return Carriage.Equals(other.Carriage) && Seats.SequenceEqual(other.Seats);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((CarriageSeatsModel)obj);
    }

    public object Clone()
    {
        var clone = new CarriageSeatsModel { Carriage = Carriage, Seats = new List<SeatModel>() };
        foreach (var seat in Seats)
            clone.Seats.Add(new SeatModel { NumberOfSeats = seat.NumberOfSeats, IsFree = seat.IsFree });
        
        return clone;
    }
}