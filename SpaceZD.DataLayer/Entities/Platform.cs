namespace SpaceZD.DataLayer.Entities
{
    public class Platform
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public Station Station { get; set; }
        public bool IsDeleted { get; set; }
        public List<NotWorkPlatform> NotWorkPlatforms { get; set; }
    }
}
