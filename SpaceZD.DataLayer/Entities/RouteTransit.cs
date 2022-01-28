namespace SpaceZD.DataLayer.Entities;

public class RouteTransit
{
    public         int                Id            {get; set;}
    public virtual Transit            Transit       {get; set;}
    public         TimeSpan           DepartingTime {get; set;}
    public         TimeSpan           ArrivalTime   {get; set;}
    public         bool               IsDeleted     {get; set;}
    public virtual ICollection<Route> Routes        {get; set;}
}