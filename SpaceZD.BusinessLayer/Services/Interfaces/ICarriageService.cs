using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ICarriageService
    {
        int Add(CarriageModel carriage);
        void Delete(int id);
        CarriageModel GetById(int id);
        List<CarriageModel> GetList(bool allIncluded);
        void Restore(int id);
        void Update(int id, CarriageModel carriage);
    }
}