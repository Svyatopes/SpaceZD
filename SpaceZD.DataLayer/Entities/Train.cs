namespace SpaceZD.DataLayer.Entities;

public class Train
{
    public int Id { get; set; }
    public virtual ICollection<Carriage> Carriages { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Trip> Trips { get; set; }
}