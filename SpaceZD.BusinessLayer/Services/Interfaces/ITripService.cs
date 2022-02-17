using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services;

public interface ITripService
{
    List<CarriageSeatsModel> GetFreeSeat(int idTrip, int idStartStation, int idEndStation);
}