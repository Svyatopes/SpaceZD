namespace SpaceZD.DataLayer.Entities;

public class CarriageType
{
    public         int                   Id            {get; set;}
    public         string                Name          {get; set;}
    public         int                   NumberOfSeats {get; set;}
    public         bool                  IsDeleted     {get; set;}
    public virtual ICollection<Carriage> Carriages     {get; set;}
}