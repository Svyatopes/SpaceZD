using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.BusinessLayer.Tests.Configuration;
using SpaceZD.BusinessLayer.Tests.Helpers;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;

namespace SpaceZD.BusinessLayer.Tests;

public class AuthServiceTests
{
    private Mock<ILoginUser> _loginUserMock;

    [SetUp]
    public void Setup()
    {
        _loginUserMock = new Mock<ILoginUser>();
    }

    [Test]
    public void LoginTest()
    {
        //given
        var login = "CoachPotato1861";
        var password = "MySuperPassword";

        _loginUserMock.Setup(a => a.GetUserByLogin(It.IsAny<string>())).Returns(new User
        {
            Id = 1,
            Login = login,
            PasswordHash = SecurePasswordHasher.Hash(password)
        });


        var _authService = new AuthService(_loginUserMock.Object);

        //when

        var token = _authService.Login(login, password);


        //then
        Assert.IsNotNull(token);
        Assert.IsTrue(ValidateToken(token));
    }

    [Test]
    public void LoginTest_NotFoundException()
    {
        //given
        var login = "CoachPotato1861";
        var password = "MySuperPassword";

        _loginUserMock.Setup(a => a.GetUserByLogin(It.IsAny<string>())).Returns(default(User));
        var _authService = new AuthService(_loginUserMock.Object);

        //when then
        Assert.Throws<NotFoundException>(() => _authService.Login(login, password));
    }


    [Test]
    public void LoginTest_AuthenticationException()
    {
        //given
        var login = "CoachPotato1861";
        var password = "MySuperPassword";
        var wrongPassword = "MyMegaPassword";

        _loginUserMock.Setup(a => a.GetUserByLogin(It.IsAny<string>())).Returns(new User
        {
            Id = 1,
            Login = login,
            PasswordHash = SecurePasswordHasher.Hash(password)
        });

        var _authService = new AuthService(_loginUserMock.Object);

        //when then
        Assert.Throws<AuthenticationException>(() => _authService.Login(login, wrongPassword));
    }

    private static bool ValidateToken(string authToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        SecurityToken validatedToken;
        IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
        return true;
    }

    private static TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    }
}
