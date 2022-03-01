using Microsoft.IdentityModel.Tokens;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.DataLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpaceZD.BusinessLayer.Services;

public class AuthService : IAuthService
{
    private readonly ILoginUser _userRepository;

    public AuthService(ILoginUser userRepository)
    {
        _userRepository = userRepository;
    }

    public string Login(string login, string password)
    {
        var user = _userRepository.GetUserByLogin(login);
        if (user == null || user.IsDeleted)
            throw new NotFoundException($"User with login {login} was not found.");

        if (!SecurePasswordHasher.Verify(password, user.PasswordHash))
            throw new AuthenticationException("Password is not correct for this user.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.UserData, user.Id.ToString()),
            new(ClaimTypes.Name, user.Login),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}