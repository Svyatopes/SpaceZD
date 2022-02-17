using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripService
{
    TripModel GetById(int id);
    List<TripModel> GetList();
    List<TripModel> GetListDeleted();
    void Delete(int id);
    void Restore(int id);
    void Update(int id, TripModel tripModel);
    int Add(TripModel tripModel);
    List<CarriageSeatsModel> GetFreeSeat(int idTrip, int idStartStation, int idEndStation);
}