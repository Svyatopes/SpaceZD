namespace SpaceZD.DataLayer.Entities
{
    public class RouteStation
    {
        public int Id { get; set; }
        public Station Station { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartingTime { get; set; }
        public bool IsDeleted { get; set; }
    }
}
