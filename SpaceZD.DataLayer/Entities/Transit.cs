namespace SpaceZD.DataLayer.Entities;

public class Transit
{
    public         int                       Id           {get; set;}
    public virtual Station                   StartStation {get; set;}
    public virtual Station                   EndStation   {get; set;}
    public         decimal                   Price        {get; set;}
    public         bool                      IsDeleted    {get; set;}
    public virtual ICollection<RouteTransit> RouteTransit {get; set;}
}