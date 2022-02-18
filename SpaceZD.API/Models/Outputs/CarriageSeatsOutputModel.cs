namespace SpaceZD.API.Models;

public class CarriageSeatsOutputModel
{
    public CarriageOutputModel Carriage { get; set; }
    public List<SeatOutputModel> Seats { get; set; }
}