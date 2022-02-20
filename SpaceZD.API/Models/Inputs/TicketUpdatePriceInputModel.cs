namespace SpaceZD.API.Models
{
    public class TicketUpdatePriceInputModel
    {
        public int Price { get; set; }        
        public bool IsTeaIncluded { get; set; }
        public bool IsPetPlaceIncluded { get; set; }        
    }
}
