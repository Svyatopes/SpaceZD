namespace SpaceZD.BusinessLayer.Models
{
    public class TrainModel
    {
        public int Id { get; set; }
        public List<CarriageModel> Carriages { get; set; }
        public bool IsDeleted { get; set; }
    }
}
