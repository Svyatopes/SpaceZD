using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ICarriageTypeService
    {
        int Add(CarriageTypeModel carriageTypeModel);
        void Delete(int id);
        CarriageTypeModel GetById(int id);
        List<CarriageTypeModel> GetList();
        List<CarriageTypeModel> GetListDeleted();
        void Restore(int id);
        void Update(int id, CarriageTypeModel carriageTypeModel);
    }
}