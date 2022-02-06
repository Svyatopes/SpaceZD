namespace SpaceZD.BusinessLayer.Models
{
    public class StationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public List<StationModel> NearStations { get; set; }
        public List<PlatformModel> Platforms { get; set; }
        public bool IsDeleted { get; set; }
    }
}
