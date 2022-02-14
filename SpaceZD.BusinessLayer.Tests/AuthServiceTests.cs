using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.BusinessLayer.Tests.Configuration;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace SpaceZD.BusinessLayer.Tests;

public class AuthServiceTests
{
    private Mock<ILoginUser> _loginUserMock;
    private const string _login = "CoachPotato1861";
    private const string _password = "MySuperPassword";

    [SetUp]
    public void Setup()
    {
        _loginUserMock = new Mock<ILoginUser>();
    }

    [Test]
    public void LoginTest()
    {
        //given
        _loginUserMock.Setup(a => a.GetUserByLogin(It.IsAny<string>())).Returns(new User
        {
            Id = 1,
            Login = _login,
            PasswordHash = SecurePasswordHasher.Hash(_password)
        });

        var authService = new AuthService(_loginUserMock.Object);

        //when
        var token = authService.Login(_login, _password);

        //then
        Assert.IsNotNull(token);
        Assert.IsTrue(ValidateToken(token));
    }

    [Test]
    public void LoginTest_NotFoundException()
    {
        //given
        _loginUserMock.Setup(a => a.GetUserByLogin(It.IsAny<string>())).Returns(default(User));
        var authService = new AuthService(_loginUserMock.Object);

        //when then
        Assert.Throws<NotFoundException>(() => authService.Login(_login, _password));
    }


    [Test]
    public void LoginTest_AuthenticationException()
    {
        //given
        var wrongPassword = "MyMegaPassword";

        _loginUserMock.Setup(a => a.GetUserByLogin(It.IsAny<string>())).Returns(new User
        {
            Id = 1,
            Login = _login,
            PasswordHash = SecurePasswordHasher.Hash(_password)
        });

        var authService = new AuthService(_loginUserMock.Object);

        //when then
        Assert.Throws<AuthenticationException>(() => authService.Login(_login, wrongPassword));
    }

    private static bool ValidateToken(string authToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(authToken, GetValidationParameters(), out _);

        return true;
    }

    private static TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
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