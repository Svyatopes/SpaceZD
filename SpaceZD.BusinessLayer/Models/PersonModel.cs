namespace SpaceZD.BusinessLayer.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Passport { get; set; }
        public bool IsDeleted { get; set; }

        private bool Equals(PersonModel other)
        {
            return FirstName == other.FirstName &&
                LastName == other.LastName &&
                Patronymic == other.Patronymic &&
                Passport == other.Passport &&
                IsDeleted == other.IsDeleted;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((PersonModel)obj);
        }

    }
}
