namespace SpaceZD.BusinessLayer.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        //public Trip Trip { get; set; }
        //public TripStation StartStation { get; set; }
        //public TripStation EndStation { get; set; }
        public List<TicketModel> Tickets { get; set; }
        public bool IsDeleted { get; set; }
    }
}
