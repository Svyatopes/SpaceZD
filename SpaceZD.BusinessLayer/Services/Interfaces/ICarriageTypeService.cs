using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ICarriageTypeService
{
    CarriageTypeModel GetById(int userId, int id);
    List<CarriageTypeModel> GetList();
    List<CarriageTypeModel> GetListDeleted(int userId);
    int Add(int userId, CarriageTypeModel carriageTypeModel);
    void Delete(int userId, int id);
    void Restore(int userId, int id);
    void Update(int userId, int id, CarriageTypeModel carriageTypeModel);
}