namespace SpaceZD.DataLayer.Entities;

public class Carriage
{
    public         int                 Id        {get; set;}
    public         int                 Number    {get; set;}
    public virtual CarriageType        Type      {get; set;}
    public         bool                IsDeleted {get; set;}
    public virtual Train               Train     {get; set;}
    public virtual ICollection<Ticket> Tickets   {get; set;}
}