namespace SpaceZD.DataLayer.Entities;

public class Ticket
{
    public int Id { get; set; }
    public virtual Order Order { get; set; }
    public virtual Carriage Carriage { get; set; }
    public int SeatNumber { get; set; }
    public bool IsTeaIncluded { get; set; }
    public bool IsPetPlaceIncluded { get; set; }
    public virtual Person Person { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }
}