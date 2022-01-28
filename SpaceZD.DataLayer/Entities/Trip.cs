namespace SpaceZD.DataLayer.Entities
{
    public class Trip
    {
        public int Id { get; set; }
        public Train Train { get; set; }
        public Route Route { get; set; }
        public LinkedList<TripStation> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Order> Orders { get; set; }

    }
}
