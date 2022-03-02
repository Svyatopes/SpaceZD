using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Enums;

namespace SpaceZD.BusinessLayer.Services;

public interface IUserService : IDeleteRestoreUpdate<UserModel>, IGetByIdWithUserId<UserModel>
{
    int Add(UserModel entity, string password);
    UserModel GetByLogin(string login, int userId);
    List<UserModel> GetList(int userId);
    List<UserModel> GetListDelete(int userId);
    void UpdateRole(int id, Role role, int userId);
    void UpdatePassword(string passwordOld, string passwordNew, int userId);
}