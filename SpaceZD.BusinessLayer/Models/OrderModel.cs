namespace SpaceZD.BusinessLayer.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public TripModel Trip { get; set; }
        public TripStationModel StartStation { get; set; }
        public TripStationModel EndStation { get; set; }
        public List<TicketModel> Tickets { get; set; }
        public bool IsDeleted { get; set; }
    }
}
