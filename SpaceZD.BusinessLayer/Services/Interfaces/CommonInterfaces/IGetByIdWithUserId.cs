namespace SpaceZD.BusinessLayer.Services;

public interface IGetByIdWithUserId<out T> where T : class
{
    T GetById(int userId, int id);
}