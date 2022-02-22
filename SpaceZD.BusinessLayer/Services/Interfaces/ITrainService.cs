using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITrainService
    {
        int Add(TrainModel entity);
        TrainModel GetById(int id);
        List<TrainModel> GetList(bool includeAll = false);
        List<TrainModel> GetListDeleted(bool includeAll = true);
        void Restore(int id);
        void Delete(int id);
        void Update(int id, TrainModel entity);
    }
}