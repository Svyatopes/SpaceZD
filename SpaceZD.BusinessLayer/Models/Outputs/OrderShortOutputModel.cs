namespace SpaceZD.BusinessLayer.Models
{
    public class OrderShortOutputModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TripId { get; set; }
        public int StartTripStationId { get; set; }
        public int EndTripStationId { get; set; }
    }
}
