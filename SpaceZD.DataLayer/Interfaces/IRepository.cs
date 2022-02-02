namespace SpaceZD.DataLayer.Interfaces
{
    public interface IRepository<T>
    {
        public void Add(T model);
        public T? GetEntity(int id);
        public bool Update(T model);
    }
}
