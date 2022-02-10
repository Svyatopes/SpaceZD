namespace SpaceZD.BusinessLayer.Exceptions;

public class AuthorizationException : Exception
{
    public AuthorizationException(string message) : base(message) { }
}