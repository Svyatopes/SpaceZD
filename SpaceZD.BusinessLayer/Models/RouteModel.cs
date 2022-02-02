namespace SpaceZD.BusinessLayer.Models
{
    public class RouteModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        //public List<RouteTransit> Transits { get; set; }
        public DateTime StartTime { get; set; }
        //public virtual Station StartStation { get; set; }
        // public virtual Station EndStation { get; set; }
        public bool IsDeleted { get; set; }
        // public virtual ICollection<Trip> Trips { get; set; }
    }
}
