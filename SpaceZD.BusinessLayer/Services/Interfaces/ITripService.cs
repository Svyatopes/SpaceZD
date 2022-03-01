using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripService : IDeleteRestoreUpdate<TripModel>, IAddWithUserId<TripModel>
{
    TripModel GetById(int id);
    List<TripModel> GetList();
    List<TripModel> GetListDeleted(int userId);
    List<CarriageSeatsModel> GetFreeSeat(int idTrip, int idStartStation, int idEndStation);
}