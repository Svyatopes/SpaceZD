using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ICarriageService : IDeleteRestoreUpdate<CarriageModel>, IGetByIdWithUserId<CarriageModel>, IAddWithUserId<CarriageModel>
{
    List<CarriageModel> GetList(int userId);
    List<CarriageModel> GetListDeleted(int userId);
}