namespace SpaceZD.API.Models.Inputs
{
    public class TransitInputModel
    {
        public virtual StationModel StartStation { get; set; }
        public virtual StationModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public virtual ICollection<RouteTransitModel> RouteTransit { get; set; }
    }
}
