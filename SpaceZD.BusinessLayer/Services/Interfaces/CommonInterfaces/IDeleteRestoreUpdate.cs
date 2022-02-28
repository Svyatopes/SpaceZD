namespace SpaceZD.BusinessLayer.Services;

public interface IDeleteRestoreUpdate<in T> where T : class
{
    void Delete(int userId, int id);
    void Restore(int userId, int id);
    void Update(int userId, int id, T model);
}