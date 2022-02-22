using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripService
{
    TripModel GetById(int id);
    List<TripModel> GetList();
    List<TripModel> GetListDeleted(int userId);
    void Delete(int userId, int id);
    void Restore(int userId, int id);
    void Update(int userId, int id, TripModel tripModel);
    int Add(int userId, TripModel tripModel);
    List<CarriageSeatsModel> GetFreeSeat(int idTrip, int idStartStation, int idEndStation);
}