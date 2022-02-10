using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Helpers;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

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
}
