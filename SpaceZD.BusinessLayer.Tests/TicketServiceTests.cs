using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services;
using SpaceZD.BusinessLayer.Tests.TestCaseSources;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests;

public class TicketServiceTests
{
    private Mock<IRepositorySoftDelete<Ticket>> _ticketRepositoryMock;
    private readonly IMapper _mapper;

    public TicketServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _ticketRepositoryMock = new Mock<IRepositorySoftDelete<Ticket>>();
    }


    
    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetListTestCases))]
    
    public void GetListTest(List<Ticket> tickets, List<TicketModel> expectedTicketModels, bool allIncluded)
    {
        // given
        var ticketsFiltredByIsDeletedProp = tickets.Where(o => !o.IsDeleted || allIncluded).ToList();
        _ticketRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(ticketsFiltredByIsDeletedProp);

        expectedTicketModels = expectedTicketModels.Where(o => !o.IsDeleted || allIncluded).ToList();

        var ticketService = new TicketService(_mapper, _ticketRepositoryMock.Object);

        // when
        var ticketModels = ticketService.GetList(allIncluded);

        // then
        CollectionAssert.AreEqual(expectedTicketModels, ticketModels);
        _ticketRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
    }


    [TestCaseSource(typeof(TicketServiceTestCaseSource), nameof(TicketServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Ticket ticket, TicketModel expected)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        var service = new TicketService(_mapper, _ticketRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        Assert.AreEqual(expected, actual);
        
    }

    
    [TestCase(5)]
    public void AddTest(int expected)
    {
        // given
        _ticketRepositoryMock.Setup(x => x.Add(It.IsAny<Ticket>())).Returns(expected);
        var service = new TicketService(_mapper, _ticketRepositoryMock.Object);

        // when
        int actual = service.Add(new TicketModel
        {
            Person = new PersonModel() { Id = 1 },
            Carriage = new CarriageModel() { Id = 2 }            
        });

        // then
        _ticketRepositoryMock.Verify(s => s.Add(It.IsAny<Ticket>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }


    [Test]
    public void UpdateTest()
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.Update(It.IsAny<Ticket>(), It.IsAny<Ticket>()));
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        var service = new TicketService(_mapper, _ticketRepositoryMock.Object);

        // when
        service.Update(5, new TicketModel());

        // then
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, It.IsAny<Ticket>()), Times.Once);
    }


    [Test]
    public void DeleteTest()
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.Update(It.IsAny<Ticket>(), true));
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        var service = new TicketService(_mapper, _ticketRepositoryMock.Object);

        // when
        service.Delete(5);

        // then
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, true), Times.Once);
    }



    
    [Test]
    public void RestoreTest()
    {
        // given
        var ticket = new Ticket();
        _ticketRepositoryMock.Setup(x => x.Update(It.IsAny<Ticket>(), false));
        _ticketRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(ticket);
        var service = new TicketService(_mapper, _ticketRepositoryMock.Object);

        // when
        service.Restore(5);

        // then
        _ticketRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _ticketRepositoryMock.Verify(s => s.Update(ticket, false), Times.Once);
    }

}