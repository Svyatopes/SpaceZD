using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ICarriageService
    {
        int Add(int userId, CarriageModel carriageModel);
        void Delete(int userId, int id);
        CarriageModel GetById(int userId, int id);
        List<CarriageModel> GetList(int userId);
        List<CarriageModel> GetListDeleted(int userId);
        void Restore(int userId, int id);
        void Update(int userId, int id, CarriageModel carriageModel);
    }
}