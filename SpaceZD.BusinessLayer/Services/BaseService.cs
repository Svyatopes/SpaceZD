using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public abstract class BaseService
{
    protected readonly IMapper _mapper;
    protected readonly IUserRepository _userRepository;

    protected BaseService(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
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

    protected User CheckUserRole(int userId, params Role[] roles)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        if (!roles.Contains(user!.Role) || user.IsDeleted)
            ThrowIfRoleDoesntHavePermissions();

        return user;
    }
}