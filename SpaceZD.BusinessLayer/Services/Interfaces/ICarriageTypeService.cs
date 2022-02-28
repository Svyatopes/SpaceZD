using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ICarriageTypeService : IDeleteRestoreUpdate<CarriageTypeModel>, IGetByIdWithUserId<CarriageTypeModel>, IAddWithUserId<CarriageTypeModel>
{
    List<CarriageTypeModel> GetList();
    List<CarriageTypeModel> GetListDeleted(int userId);
}