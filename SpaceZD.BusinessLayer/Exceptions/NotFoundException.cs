namespace SpaceZD.BusinessLayer.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }

    public static void ThrowEntityNotFound(string entityName, int id) => throw new NotFoundException($"{entityName} c Id = {id} не найден");
}