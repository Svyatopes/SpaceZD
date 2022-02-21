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

public class TrainServiceTests
{
    private Mock<IRepositorySoftDelete<Train>> _trainRepositoryMock;
    private Mock<IRepositorySoftDelete<User>> _userRepositoryMock;
    private readonly IMapper _mapper;

    public TrainServiceTests()
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
    }

    [SetUp]
    public void SetUp()
    {
        _trainRepositoryMock = new Mock<IRepositorySoftDelete<Train>>();
        _userRepositoryMock = new Mock<IRepositorySoftDelete<User>>();
    }



    [TestCaseSource(typeof(TrainServiceTestCaseSource), nameof(TrainServiceTestCaseSource.GetListTestCases))]

    public void GetListTest(List<Train> trains, List<TrainModel> expectedTrainModels, bool allIncluded)
    {
        // given
        var trainsFiltredByIsDeletedProp = trains.Where(o => !o.IsDeleted || allIncluded).ToList();
        _trainRepositoryMock.Setup(x => x.GetList(It.IsAny<bool>()))
            .Returns(trainsFiltredByIsDeletedProp);

        expectedTrainModels = expectedTrainModels.Where(o => !o.IsDeleted || allIncluded).ToList();

        var trainService = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);

        // when
        var trainModels = trainService.GetList(allIncluded);

        // then
        CollectionAssert.AreEqual(expectedTrainModels, trainModels);
        _trainRepositoryMock.Verify(s => s.GetList(It.IsAny<bool>()), Times.Once);
    }


    [TestCaseSource(typeof(TrainServiceTestCaseSource), nameof(TrainServiceTestCaseSource.GetByIdTestCases))]
    public void GetByIdTest(Train train, TrainModel expected)
    {
        // given
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(train);
        var service = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);

        // when
        var actual = service.GetById(5);

        // then
        Assert.AreEqual(expected, actual);

    }



    [TestCase(5)]
    public void AddTest(int expected)
    {
        // given
        _trainRepositoryMock.Setup(x => x.Add(It.IsAny<Train>())).Returns(expected);
        var service = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);

        // when
        int actual = service.Add(new TrainModel
        {
            Carriages = new List<CarriageModel>()
            {
                new CarriageModel()
                {
                    Number = 8,
                    IsDeleted = false,
                    Type = new CarriageTypeModel()
                    {
                        Name = "Эконом",
                        NumberOfSeats = 5,
                        IsDeleted = false
                    }
                }
            }        
        });

        // then
        _trainRepositoryMock.Verify(s => s.Add(It.IsAny<Train>()), Times.Once);
        Assert.AreEqual(expected, actual);
    }


    [Test]
    public void UpdateTest()
    {
        // given
        var train = new Train();
        _trainRepositoryMock.Setup(x => x.Update(It.IsAny<Train>(), It.IsAny<Train>()));
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(train);
        var service = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);

        // when
        service.Update(5, new TrainModel());

        // then
        _trainRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _trainRepositoryMock.Verify(s => s.Update(train, It.IsAny<Train>()), Times.Once);
    }

   


    [Test]
    public void DeleteTest()
    {
        // given
        var train = new Train();
        _trainRepositoryMock.Setup(x => x.Update(It.IsAny<Train>(), true));
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(train);
        var service = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);

        // when
        service.Delete(5);

        // then
        _trainRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _trainRepositoryMock.Verify(s => s.Update(train, true), Times.Once);
    }

   

    //Restore
    [Test]
    public void RestoreTest()
    {
        // given
        var train = new Train();
        _trainRepositoryMock.Setup(x => x.Update(It.IsAny<Train>(), false));
        _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(train);
        var service = new TrainService(_mapper, _userRepositoryMock.Object, _trainRepositoryMock.Object);

        // when
        service.Restore(5);

        // then
        _trainRepositoryMock.Verify(s => s.GetById(5), Times.Once);
        _trainRepositoryMock.Verify(s => s.Update(train, false), Times.Once);
    }
}