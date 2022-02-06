namespace SpaceZD.DataLayer.Interfaces;

public interface IRepositorySoftDelete<T> where T : class
{
    public T? GetById(int id);
    public IEnumerable<T> GetList(bool includeAll = false);
    public int Add(T model);
    public bool Update(T model);
    public bool Update(int id, bool isDeleted);
}