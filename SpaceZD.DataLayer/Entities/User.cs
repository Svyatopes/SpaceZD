using SpaceZD.DataLayer.Enums;

namespace SpaceZD.DataLayer.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
    public Role Role { get; set; }
    public bool IsDeleted { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is User user &&
               Name == user.Name &&
               Login == user.Login &&
               PasswordHash == user.PasswordHash &&
               Orders.Equals(user.Orders) &&
               Role == user.Role &&
               IsDeleted == user.IsDeleted;
    }
}