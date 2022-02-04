namespace SpaceZD.DataLayer.Interfaces
{
    public interface ISoftDelete<T>
    {
        public List<T> GetList(bool allIncluded);
        public bool Update(int id, bool isDeleted);
    }
}
