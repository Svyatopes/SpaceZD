namespace SpaceZD.BusinessLayer.Models
{
    public class RouteTransitModel
    {
        public int Id { get; set; }
        public TransitModel Transit { get; set; }
        public TimeSpan DepartingTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public bool IsDeleted { get; set; }
        
        private bool Equals(RouteTransitModel other)
        {
            return Transit.Equals(other.Transit) &&
                DepartingTime.Equals(other.DepartingTime) &&
                ArrivalTime.Equals(other.ArrivalTime) &&
                IsDeleted == other.IsDeleted;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((RouteTransitModel)obj);
        }
    }
}