namespace SpaceZD.API.Models
{
    public class RouteUpdateInputModel
    {
        public DateTime StartTime { get; set; }
        public int StartStationId { get; set; }
        public int EndStationId { get; set; }
    }
}
