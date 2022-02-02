namespace SpaceZD.BusinessLayer.Models
{
    public class RouteTransitModel
    {
        public int Id { get; set; }
        //public  Transit Transit { get; set; }
        public TimeSpan DepartingTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public bool IsDeleted { get; set; }
        public RouteModel Route { get; set; }
    }
}
