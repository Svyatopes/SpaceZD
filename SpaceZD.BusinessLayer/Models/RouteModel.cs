namespace SpaceZD.BusinessLayer.Models
{
    public class RouteModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public List<RouteTransitModel> Transits { get; set; }
        public DateTime StartTime { get; set; }
        public StationModel StartStation { get; set; }
        public StationModel EndStation { get; set; }
        public bool IsDeleted { get; set; }
    }
}
