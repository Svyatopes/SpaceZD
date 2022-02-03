namespace SpaceZD.API.Models.Outputs
{
    public class TransitOutputModel
    {
        public int Id { get; set; }
        public virtual StationModel StartStation { get; set; }
        public virtual StationModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<RouteTransitModel> RouteTransit { get; set; }
    }
}
