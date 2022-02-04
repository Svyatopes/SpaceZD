namespace SpaceZD.BusinessLayer.Models
{
    public class TransitModel
    {
        public int Id { get; set; }
        public virtual StationModel StartStation { get; set; }
        public virtual StationModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}
