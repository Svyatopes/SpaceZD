namespace SpaceZD.API.Models.Outputs
{
    public class TransitOutputModel
    {
        public int Id { get; set; }
        public StationOutputModel StartStation { get; set; }
        public StationOutputModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<RouteTransitOutputModel> RouteTransit { get; set; }
    }
}
