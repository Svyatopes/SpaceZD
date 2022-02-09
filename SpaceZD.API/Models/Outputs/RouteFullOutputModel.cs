namespace SpaceZD.API.Models
{
    public class RouteFullOutputModel: RouteShortOutputModel
    {
        public List<RouteTransitModel> Transits { get; set; }
    }
}
