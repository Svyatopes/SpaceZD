using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface IPersonService
    {
        int Add(PersonModel entity, string login);
        PersonModel GetById(int id);
        List<PersonModel> GetByUserLogin(string login);
        List<PersonModel> GetList(bool includeAll = false);
        List<PersonModel> GetListDeleted(bool includeAll = true);
        void Update(int id, bool isDeleted, string login);
        void Update(int id, PersonModel entity, string login);
    }
}