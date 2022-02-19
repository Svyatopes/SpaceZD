using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITransitService
    {
        int Add(TransitModel transitModel);
        void Delete(int id);
        TransitModel GetById(int id);
        List<TransitModel> GetList(bool includeAll = false);
        List<TransitModel> GetListDeleted(bool includeAll = true);
        void Restore(int id);
        void Update(int id, TransitModel transitModel);
    }
}
