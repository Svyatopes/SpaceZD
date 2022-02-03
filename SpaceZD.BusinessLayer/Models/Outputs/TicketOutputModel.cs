namespace SpaceZD.BusinessLayer.Models
{
    public class TicketOutputModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int CarriageId { get; set; }
        public int SeatNumber { get; set; }
        public bool IsTeaIncluded { get; set; }
        public bool IsPetPlaceIncluded { get; set; }
        public int PersonId { get; set; }
    }
}
