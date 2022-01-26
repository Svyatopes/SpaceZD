namespace SpaceZD.DataLayer.Entities
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Station> NearStation { get; set; }
        public List<Platform> Platforms { get; set; }
        public bool IsDeleted { get; set; }
    }
}
