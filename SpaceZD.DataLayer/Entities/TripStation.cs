namespace SpaceZD.DataLayer.Entities
{
    public class TripStation
    {
        public int Id { get; set; }
        public Station Station { get; set; }
        public Platform Platform { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartingTime { get; set; }
        public List<Order> Orders { get; set; }
    }
}
