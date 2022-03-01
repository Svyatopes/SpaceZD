using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.DataLayer.Interfaces;

public interface IUserRepository : IRepositorySoftDelete<User>
{
    User? GetByLogin(string login);
    List<Person> GetListUserPersons(int id);
    void UpdateRole(User user, Role role);
    void UpdatePassword(User user, string passwordHash);
}
