namespace SpaceZD.BusinessLayer.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public OrderModel Order { get; set; }
        public CarriageModel Carriage { get; set; }
        public int SeatNumber { get; set; }
        public bool IsTeaIncluded { get; set; }
        public bool IsPetPlaceIncluded { get; set; }
        public PersonModel Person { get; set; }
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
