namespace SpaceZD.API.Models.Outputs
{
    public class TransitOutputModel
    {
        public int Id { get; set; }
        public StationModel StartStation { get; set; }
        public StationModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<RouteTransitModel> RouteTransit { get; set; }
    }
}
