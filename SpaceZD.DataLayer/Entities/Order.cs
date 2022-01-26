namespace SpaceZD.DataLayer.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Trip Trip { get; set; }
        public TripStation StartStation { get; set; }
        public TripStation EndStation { get; set; }
        public List<Ticket> Tickets { get; set; }
        public bool IsDeleted { get; set; }
    }
}
