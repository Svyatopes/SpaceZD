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

        public bool Equals(TicketModel other)
        {
            return other is TicketModel model &&
                   Person.Equals(model.Person) &&
                   Order.StartStation.Station.Name == model.Order.StartStation.Station.Name &&
                   Carriage.Number == model.Carriage.Number &&
                   SeatNumber == model.SeatNumber &&
                   Price == model.Price &&
                   IsDeleted == model.IsDeleted;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((TicketModel)obj);
        }
    }

}
