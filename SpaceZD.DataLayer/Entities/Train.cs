namespace SpaceZD.DataLayer.Entities
{
    public class Train
    {
        public int Id { get; set; }
        public List<Carriage> Carriages { get; set; }
        public List<Route> Routes { get; set; }        
        public List<Trip> Trips { get; set; }
        public bool IsDeleted { get; set; }
    }
}
