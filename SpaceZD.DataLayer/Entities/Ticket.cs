namespace SpaceZD.DataLayer.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public Carriage Carriage { get; set; }
        public int SeatNumber { get; set; }
        public Trip Trip { get; set; }
        public bool IsTeaIncluded { get; set; }
        public bool IsPetPlaceIncluded { get; set; }
        public Person Person { get; set; }
    }
}
