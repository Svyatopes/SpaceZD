using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface IUserRepository : IRepositorySoftDelete<User>
{
    User GetByLogin(string login);
    List<Person> GetListUserPersons(int id);
}
