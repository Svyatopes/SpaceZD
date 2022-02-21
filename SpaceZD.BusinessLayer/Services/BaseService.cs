using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;

namespace SpaceZD.BusinessLayer.Services;

public abstract class BaseService
{
    protected readonly IMapper _mapper;

    protected BaseService(IMapper mapper)
    {
        _mapper = mapper;
    }

    protected static void ThrowIfEntityNotFound<T>(T? entity, int id)
    {
        if (entity is null)
            throw new NotFoundException(typeof(T).Name, id);
    }

    protected static void ThrowIfRoleDoesntHavePermissions()
    {
        throw new AuthorizationException("Your current role doesn't have permissions to do this.");
    }
}