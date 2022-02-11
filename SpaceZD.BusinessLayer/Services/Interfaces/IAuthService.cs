namespace SpaceZD.BusinessLayer.Services;

public interface IAuthService
{
    string Login(string login, string password);
}