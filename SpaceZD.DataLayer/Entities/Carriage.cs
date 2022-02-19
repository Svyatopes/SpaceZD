namespace SpaceZD.DataLayer.Entities;

public class Carriage
{
    public int Id { get; set; }
    public int Number { get; set; }
    public virtual CarriageType Type { get; set; }
    public bool IsDeleted { get; set; }
    public virtual Train Train { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Carriage carriage &&
               Id == carriage.Id &&
               Number == carriage.Number &&
               EqualityComparer<CarriageType>.Default.Equals(Type, carriage.Type) &&
               IsDeleted == carriage.IsDeleted &&
               EqualityComparer<Train>.Default.Equals(Train, carriage.Train) &&
               EqualityComparer<ICollection<Ticket>>.Default.Equals(Tickets, carriage.Tickets);
    }
}