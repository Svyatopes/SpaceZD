using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPersonService
    {
        int Add(PersonModel entity, int userId);
        void Delete(int id, int userId);
        PersonModel GetById(int id, int userId);
        List<PersonModel> GetByUserId(int userId);
        List<PersonModel> GetList(int userId);
        List<PersonModel> GetListDeleted(int userId);
        void Restore(int id, int userId);
        void Update(int id, PersonModel entity, int userId);
    }
}