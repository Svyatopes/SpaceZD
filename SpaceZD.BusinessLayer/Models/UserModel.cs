using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public List<OrderModel> Orders { get; set; }
        public Role Role { get; set; }
        public List<PersonModel> Persons { get; set; }
        public bool IsDeleted { get; set; }

        private bool Equals(UserModel other)
        {
            return Name == other.Name && Login == other.Login && IsDeleted == other.IsDeleted;
        }
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == this.GetType() && Equals((UserModel)obj);
        }

    }
    
}
