namespace SpaceZD.DataLayer.Entities;

public class TripStation
{
    public         int                Id            {get; set;}
    public virtual Station            Station       {get; set;}
    public virtual Platform           Platform      {get; set;}
    public         DateTime           ArrivalTime   {get; set;}
    public         DateTime           DepartingTime {get; set;}
    public virtual Trip               Trip          {get; set;}
    public virtual ICollection<Order> Orders        {get; set;}
}