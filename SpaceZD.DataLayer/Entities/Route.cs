namespace SpaceZD.DataLayer.Entities
{
    public class Route
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public List<Train> Trains { get; set; }
        public LinkedList<RouteStation> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public Station StartStation { get; set; }
        public Station EndStation { get; set; }
        public decimal PricePerStation { get; set; }       
        public List<Trip> Trips { get; set; }
        public bool IsDeleted { get; set; }
    }
}
