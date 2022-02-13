using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IUserService
    {
        int Add(UserModel entity);
        UserModel GetById(int id);
        List<UserModel> GetList(bool includeAll = false);
        List<UserModel> GetListDeleted(bool includeAll = true);
        void Update(int id, bool isDeleted);
        void Update(int id, UserModel entity);
    }
}