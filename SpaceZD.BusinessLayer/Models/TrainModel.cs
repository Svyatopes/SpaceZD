namespace SpaceZD.BusinessLayer.Models
{
    public class TrainModel
    {
        public int Id { get; set; }
        public List<CarriageModel> Carriages { get; set; }
        public bool IsDeleted { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is TrainModel model &&
                   Carriages.SequenceEqual(model.Carriages) &&
                   IsDeleted == model.IsDeleted;
        }
    }
}
