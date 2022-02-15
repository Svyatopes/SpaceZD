namespace SpaceZD.BusinessLayer.Models
{
    public class TransitModel
    {
        public int Id { get; set; }
        public StationModel StartStation { get; set; }
        public StationModel EndStation { get; set; }
        public decimal? Price { get; set; }
        public bool IsDeleted { get; set; }

        private bool Equals(TransitModel other)
        {
            return StartStation.Equals(other.StartStation) &&
                EndStation.Equals(other.EndStation) &&
                Price == other.Price &&
                IsDeleted == other.IsDeleted;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((TransitModel)obj);
        }
    }
}