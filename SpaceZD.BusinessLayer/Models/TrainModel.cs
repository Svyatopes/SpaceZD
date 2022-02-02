namespace SpaceZD.BusinessLayer.Models
{
    public class TrainModel
    {
        public int Id { get; set; }
        public List<Carriage> Carriages { get; set; }
        public bool IsDeleted { get; set; }
        public List<Trip> Trips { get; set; }
    }
}
