using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITransitService
    {
        public TransitModel GetById(int id);

        public List<TransitModel> GetList();

        public List<TransitModel> GetListDeleted();

        public int Add(TransitModel transitModel);

        public void Delete(int id);

        public void Restore(int id);

        public void Update(int id, TransitModel transitModel);
    }
}
