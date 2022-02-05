using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        //public List<Order> Orders { get; set; }
        public Role Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
