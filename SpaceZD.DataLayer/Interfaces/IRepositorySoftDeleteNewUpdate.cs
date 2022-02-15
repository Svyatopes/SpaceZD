namespace SpaceZD.DataLayer.Interfaces;

public interface IRepositorySoftDeleteNewUpdate<T> where T : class
{
    public T? GetById(int id);
    public List<T> GetList(bool includeAll = false);
    public int Add(T model);
    public void Update(T oldModel, T newModel);
    public void Update(T model, bool isDeleted);
}