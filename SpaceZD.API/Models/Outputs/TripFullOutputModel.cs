namespace SpaceZD.API.Models
{
    public class TripFullOutputModel : TripShortOutputModel
    {
        public List<TripStationOutputModel> Stations { get; set; }
    }
}
