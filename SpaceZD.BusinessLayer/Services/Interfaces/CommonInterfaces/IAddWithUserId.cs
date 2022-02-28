namespace SpaceZD.BusinessLayer.Services;

public interface IAddWithUserId<in T> where T : class
{
    int Add(int userId, T model);
}