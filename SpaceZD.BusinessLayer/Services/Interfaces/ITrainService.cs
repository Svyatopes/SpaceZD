using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITrainService
    {
        int Add(TrainModel entity);
        TrainModel GetById(int id, int userId);
        List<TrainModel> GetList(int userId);
        List<TrainModel> GetListDeleted(int userId);
        void Restore(int id, int userId);
        void Delete(int id, int userId);
        void Update(int id, TrainModel entity);
    }
}