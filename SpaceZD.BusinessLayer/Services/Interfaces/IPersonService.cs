using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface IPersonService : IDeleteRestoreUpdate<PersonModel>, IGetByIdWithUserId<PersonModel>, IAddWithUserId<PersonModel>
{
    List<PersonModel> GetByUserId(int userId);
    List<PersonModel> GetList(int userId);
    List<PersonModel> GetListDeleted(int userId);
}