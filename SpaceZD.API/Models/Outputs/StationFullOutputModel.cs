namespace SpaceZD.API.Models
{
    public class StationFullOutputModel : StationShortOutputModel
    {
        public List<PlatformOutputModel> Platforms { get; set; }
    }
}
