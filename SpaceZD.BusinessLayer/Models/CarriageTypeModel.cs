namespace SpaceZD.BusinessLayer.Models;

public class CarriageTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfSeats { get; set; }
    public bool IsDeleted { get; set; }
    //public List<Carriage> Carriages { get; set; }
}