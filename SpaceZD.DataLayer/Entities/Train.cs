namespace SpaceZD.DataLayer.Entities;

public class Train
{
    public int Id { get; set; }
    public List<Carriage> Carriages { get; set; }
    public bool IsDeleted { get; set; }
    public List<Trip> Trips { get; set; }
}