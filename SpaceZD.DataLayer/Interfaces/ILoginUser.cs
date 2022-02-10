using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Interfaces
{
    public interface ILoginUser
    {
        User GetUserByLogin(string login);
    }
}