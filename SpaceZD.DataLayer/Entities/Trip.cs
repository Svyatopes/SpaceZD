namespace SpaceZD.DataLayer.Entities;

public class Trip
{
    public         int                      Id        {get; set;}
    public virtual Train                    Train     {get; set;}
    public virtual Route                    Route     {get; set;}
    public virtual ICollection<TripStation> Stations  {get; set;}
    public         DateTime                 StartTime {get; set;}
    public virtual ICollection<Order>       Orders    {get; set;}
}