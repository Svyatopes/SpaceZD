using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IUserService
    {
        int Add(UserModel entity);
        UserModel GetById(int id);
        List<UserModel> GetList(bool includeAll = false);
        bool Update(int id, bool isDeleted);
        bool Update(UserModel entity);
    }
}