namespace SpaceZD.DataLayer.Interfaces;

public interface IRepository<T> where T : class
{
    public T? GetById(int id);
    public List<T> GetList();
    public int Add(T model);
    public void Update(T oldModel, T newModel);
}