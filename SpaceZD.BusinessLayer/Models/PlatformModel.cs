namespace SpaceZD.BusinessLayer.Models
{
    public class PlatformModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public StationModel Station { get; set; }
        public bool IsDeleted { get; set; }

        private bool Equals(PlatformModel other)
        {
            return Number == other.Number &&
                Station.Name == other.Station.Name &&
                IsDeleted == other.IsDeleted;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((PlatformModel)obj);
        }
    }
}
