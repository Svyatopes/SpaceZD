using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITransitService
    {
        int Add(TransitModel transitModel, int userId);
        void Delete(int id, int userId);
        TransitModel GetById(int id, int userId);
        List<TransitModel> GetList(int userId);
        List<TransitModel> GetListDeleted(int userId);
        void Restore(int id, int userId);
        void Update(int id, TransitModel transitModel, int userId);
    }
}
