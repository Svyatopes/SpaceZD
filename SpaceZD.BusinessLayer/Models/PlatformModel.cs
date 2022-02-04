namespace SpaceZD.BusinessLayer.Models
{
    public class PlatformModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public StationModel Station { get; set; }
        public bool IsDeleted { get; set; }
    }
}
