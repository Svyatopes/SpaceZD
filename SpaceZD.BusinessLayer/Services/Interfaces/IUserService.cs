using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IUserService
    {
        int Add(UserModel entity, string password);
        UserModel GetById(int id);
        UserModel GetByLogin(string login);
        List<UserModel> GetList(bool includeAll = false);
        List<PersonModel> GetListUserPersons(int id);
        List<UserModel> GetListDeleted(bool includeAll = true);
        void Update(int id, bool isDeleted);
        void Update(int id, UserModel entity);
    }
}