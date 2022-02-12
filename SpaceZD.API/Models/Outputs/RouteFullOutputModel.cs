namespace SpaceZD.API.Models
{
    public class RouteFullOutputModel: RouteShortOutputModel
    {
        public List<RouteTransitOutputModel> Transits { get; set; }
    }
}
