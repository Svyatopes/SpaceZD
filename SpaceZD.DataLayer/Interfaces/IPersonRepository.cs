using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces;

public interface IPersonRepository : IRepositorySoftDelete<Person>
{
    List<Person> GetByUserId(int userId);
}
