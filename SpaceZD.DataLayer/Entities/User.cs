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
}