using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.BusinessLayer.Tests.TestCaseSources;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class TicketServiceTests
{
    private Mock<ITicketRepository> _ticketRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IRepositorySoftDelete<Carriage>> _carriageRepositoryMock;
    private ITicketService _service;
    private readonly IMapper _mapper;

    public TicketServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _personRepositoryMock = new Mock<IPersonRepository>();
        _carriageRepositoryMock = new Mock<IRepositorySoftDelete<Carriage>>();

        _service = new TicketService
            (_mapper, _ticketRepositoryMock.Object, _userRepositoryMock.Object,
            _orderRepositoryMock.Object, _personRepositoryMock.Object, _carriageRepositoryMock.Object );
    }

        
    

    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetListTestCases))]

    public void GetListTest(List<Ticket> tickets, List<TicketModel> expectedTicketModels, bool allIncluded, int userId)
    {
        // given
        var ticketsFiltredByIsDeletedProp = tickets.Where(o => !o.IsDeleted || allIncluded).ToList();
        _ticketRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(ticketsFiltredByIsDeletedProp);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User() { Role = Role.Admin});

        expectedTicketModels = expectedTicketModels.Where(o => !o.IsDeleted || allIncluded).ToList();
                
        // when
        var ticketModels = _service.GetList(userId);

        // then
        CollectionAssert.AreEqual(expectedTicketModels, ticketModels);
        _ticketRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }


    [TestCase(42)]
    public void GetListNegativeUserNotFoundExceptionTest(int userId)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns((User?)null);
        
        //when then
        Assert.Throws<NotFoundException>(() => _service.GetList(userId));
    }


    [TestCase(42, Role.TrainRouteManager)]
    [TestCase(42, Role.StationManager)]
    [TestCase(42, Role.User)]
    public void GetListNegativeAuthorizationExceptionTest(int userId,  Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User { Role = role });
        
        //when then
        Assert.Throws<AuthorizationException>(() => _service.GetList(userId));
    }


    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetListTestCases))]

    public void GetListDeleteTest(List<Ticket> tickets, List<TicketModel> expectedTicketModels, bool allIncluded, int userId)
    {
        // given
        var ticketsFiltredByIsDeletedProp = tickets.Where(o => o.IsDeleted).ToList();
        _ticketRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(ticketsFiltredByIsDeletedProp);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User() { Role = Role.Admin });

        expectedTicketModels = expectedTicketModels.Where(o => o.IsDeleted).ToList();

        // when
        var ticketModels = _service.GetListDeleted(userId);

        // then
        CollectionAssert.AreEqual(expectedTicketModels, ticketModels);
        _ticketRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);

    }

    [TestCase(42, Role.TrainRouteManager)]
    [TestCase(42, Role.StationManager)]
    [TestCase(42, Role.User)]
    public void GetListDeleteNegativeAuthorizationExceptionTest(int userId, Role role)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User { Role = role });

        //when then
        Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(userId));
    }



    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Ticket ticket, TicketModel expected, int userId)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(new User() { Role = Role.Admin, Id = userId });

        // when
        var actual = _service.GetById(5, userId);

        // then
        Assert.AreEqual(expected, actual);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(2));

    }


    [Test]
    public void GetByIdNegativeNotFoundUserExceptionTest()
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);
        
        //when then
        Assert.Throws<NotFoundException>(() => _service.GetById(42, 42));
    }
    
    
    [Test]
    public void GetByIdNegativeNotFoundTicketExceptionTest()
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = 42});
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Ticket?)null);
        
        //when then
        Assert.Throws<NotFoundException>(() => _service.GetById(42, 42));
    }
    
    
    [TestCase(Role.StationManager, 1)]
    [TestCase(Role.TrainRouteManager, 21)]
    public void GetByIdNegativeAuthorizationExceptionTest(Role role, int userId)
    {
        //given
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = userId});
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Ticket());
        
        //when then
        Assert.Throws<AuthorizationException>(() => _service.GetById(42, userId));
    }

    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetListTestCases))]


    


    [TestCase(Role.Admin, 5)]
    public void DeleteTest(Role role, int userId)
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.Update(It.IsAny<Ticket>(), true));
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Id = userId, Role = role });

        // when
        _service.Delete(5, userId);

        // then
        _userRepositoryMock.Verify(s => s.GetById(userId), Times.Exactly(2));
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, true), Times.Once);
    }



    [TestCase(Role.Admin, 5)]
    public void RestoreTest(Role role, int userId)
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.Update(It.IsAny<Ticket>(), false));
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Id = userId, Role = role });
        
        // when
        _service.Restore(5, userId);

        // then
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, false), Times.Once);
    }


    [Test]
    public void RestoreNegativeUserNotFoundExceptionTest()
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

        
        // when then
        // then
        Assert.Throws<NotFoundException>(() => _service.Restore(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }
    
    
    [Test]
    public void RestoreNegativeTicketNotFoundExceptionTest()
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Ticket)null);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin, Id = 42 });

        
        // when then
        // then
        Assert.Throws<NotFoundException>(() => _service.Restore(42, 42));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
    }



    [TestCase(Role.StationManager, 1)]
    [TestCase(Role.TrainRouteManager, 21)]
    [TestCase(Role.User, 41)]

    public void RestoreNegativeAuthorizationExceptionTest(Role role, int userId)
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role, Id = userId });

        
        // when then
        // then
        Assert.Throws<AuthorizationException>(() => _service.Restore(42, userId));
        _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        _ticketRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }

}