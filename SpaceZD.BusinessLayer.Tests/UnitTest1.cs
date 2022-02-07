using Moq;
using NUnit.Framework;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class Tests
{

    // FOR SHOW HOW TO USE MOQ LIBRARY

    private readonly Mock<IRepositorySoftDelete<User>> _userRepositoryMock;
    public Tests()
    {
        _userRepositoryMock = new Mock<IRepositorySoftDelete<User>>();
    }

    [SetUp]
    public void Setup() 
    {
    }

    [Test]
    public void UserAddTest()
    {

        //_userRepositoryMock.Setup(x => x.Add(It.IsAny<User>())).Returns(23);
        //var userService = new UserService(_userRepositoryMock.Object, new Mapper(new MapperConfiguration(cfg =>
        //{
        //    cfg.AddProfile<BusinessLayerMapper>();
        //})));
        

        // 

        Assert.Pass();
    }
}