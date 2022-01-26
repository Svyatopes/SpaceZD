namespace SpaceZD.DataLayer.Entities
{
    public class Carriage
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public CarriageType Type { get; set; }
        public bool IsDeleted { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
