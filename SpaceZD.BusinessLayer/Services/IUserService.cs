using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IUserService
    {
        UserModel GetById(int id);
        bool Add(UserModel user);
    }
}