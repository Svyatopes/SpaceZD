namespace SpaceZD.API.Models
{
    public class TransitOutputModel
    {
        public int Id { get; set; }
        public StationShortOutputModel StartStation { get; set; }
        public StationShortOutputModel EndStation { get; set; }
        public decimal? Price { get; set; }
    }
}
