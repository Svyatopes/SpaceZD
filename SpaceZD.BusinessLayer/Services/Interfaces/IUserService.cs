using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IUserService
    {
        int Add(UserModel entity, string password);
        UserModel GetById(int id, int userId);
        UserModel GetByLogin(string login, int userId);
        List<UserModel> GetList(int userId);
        List<UserModel> GetListDelete(int userId);
        List<PersonModel> GetListUserPersons(int userId);        
        void Delete(int id, int userId);
        void Restore(int id, int userId);
        void Update(int Id, UserModel entity, int userId);
    }
}