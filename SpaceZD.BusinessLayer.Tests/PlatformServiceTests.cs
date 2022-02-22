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
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests
{
    public class PlatformServiceTests
    {
        private Mock<IPlatformRepository> _platformRepositoryMock;
        private Mock<IRepositorySoftDelete<User>> _userRepositoryMock;
        private Mock<IStationRepository> _stationRepositoryMock;
        private readonly IMapper _mapper;

        public PlatformServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _platformRepositoryMock = new Mock<IPlatformRepository>();
            _userRepositoryMock = new Mock<IRepositorySoftDelete<User>>();
            _stationRepositoryMock = new Mock<IStationRepository>();
        }

        [TestCaseSource(typeof(PlatformServiceTestCaseSource), nameof(PlatformServiceTestCaseSource.GetListTestCases))]
        public void GetListTest(List<PlatformModel> expected, List<Platform> platforms, Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });
            _platformRepositoryMock.Setup(x => x.GetList(It.IsAny<int>(),It.IsAny<bool>())).Returns(platforms);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object, 
                _stationRepositoryMock.Object);

            //when
            var actualPlatformModels = service.GetListByStationId(42, 42);

            //then
            CollectionAssert.AreEqual(expected, actualPlatformModels);
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetList(It.IsAny<int>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void GetListNotFoundExceptionTest()
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.GetListByStationId(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.TrainRouteManager)]
        [TestCase(Role.User)]
        public void GetListAuthorizationExceptionTest(Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<AuthorizationException>(() => service.GetListByStationId(42, 42),
                "Your current role doesn't have permissions to do this.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(PlatformServiceTestCaseSource), nameof(PlatformServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(PlatformModel expected, Platform platform, Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platform);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when
            var actualPlatformModels = service.GetById(42, 42);

            //then
            Assert.AreEqual(expected, actualPlatformModels);
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetByIdNotFoundUserExceptionTest()
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.GetById(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.TrainRouteManager)]
        [TestCase(Role.User)]
        public void GetByIdAuthorizationExceptionTest(Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<AuthorizationException>(() => service.GetById(42, 42),
                "Your current role doesn't have permissions to do this.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetByIdNotFoundPlatformExceptionTest()
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Platform?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.GetById(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(PlatformServiceTestCaseSource), nameof(PlatformServiceTestCaseSource.AddTestCases))]
        public void AddTest(PlatformModel platformToAdd, Role role)
        {
            //given
            int expectedId = 42;
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });
            _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Station());
            _platformRepositoryMock.Setup(x => x.Add(It.IsAny<Platform>())).Returns(expectedId);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when
            var actualId = service.Add(42, platformToAdd);

            //then
            Assert.AreEqual(expectedId, actualId);
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _stationRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.Add(It.IsAny<Platform>()), Times.Once);
        }

        [Test]
        public void AddNotFoundUserExceptionTest()
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Add(42, platform));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.TrainRouteManager)]
        [TestCase(Role.User)]
        public void AddAuthorizationExceptionTest(Role role)
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<AuthorizationException>(() => service.Add(42, platform),
                "Your current role doesn't have permissions to do this.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void AddIdNotFoundStationExceptionTest()
        {
            //given
            var platform = new PlatformModel { Station = new StationModel { Id = 1 } };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin });
            _stationRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Station?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Add(42, platform));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _stationRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }


        [TestCaseSource(typeof(PlatformServiceTestCaseSource), nameof(PlatformServiceTestCaseSource.EditTestCases))]
        public void EditTest(PlatformModel platformModel, Platform platform, Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platform);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when
            service.Edit(42, platformModel);

            //then
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.Update(It.IsAny<Platform>(), It.IsAny<Platform>()), Times.Once);
        }

        [Test]
        public void EditNotFoundUserExceptionTest()
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Edit(42, platform));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.TrainRouteManager)]
        [TestCase(Role.User)]
        public void EditAuthorizationExceptionTest(Role role)
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<AuthorizationException>(() => service.Edit(42, platform),
                "Your current role doesn't have permissions to do this.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void EditIdNotFoundPlatformExceptionTest()
        {
            //given
            var platform = new PlatformModel { Station = new StationModel { Id = 1 } };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Platform?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Edit(42, platform));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(PlatformServiceTestCaseSource), nameof(PlatformServiceTestCaseSource.DeleteTestCases))]
        public void DeleteTest(Platform platform, Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platform);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when
            service.Delete(42, 42);

            //then
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.Update(It.IsAny<Platform>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void DeleteNotFoundUserExceptionTest()
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Delete(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.TrainRouteManager)]
        [TestCase(Role.User)]
        public void DeleteAuthorizationExceptionTest(Role role)
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<AuthorizationException>(() => service.Delete(42, 42),
                "Your current role doesn't have permissions to do this.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void DeleteIdNotFoundPlatformExceptionTest()
        {
            //given
            var platform = new PlatformModel { Station = new StationModel { Id = 1 } };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Platform?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Delete(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCaseSource(typeof(PlatformServiceTestCaseSource), nameof(PlatformServiceTestCaseSource.RestoreTestCases))]
        public void RestoreTest(Platform platform, Role role)
        {
            //given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platform);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when
            service.Restore(42, 42);

            //then
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.Update(It.IsAny<Platform>(), It.IsAny<bool>()), Times.Once);
        }

        [Test]
        public void RestoreNotFoundUserExceptionTest()
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Restore(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [TestCase(Role.TrainRouteManager)]
        [TestCase(Role.StationManager)]
        [TestCase(Role.User)]
        public void RestoreAuthorizationExceptionTest(Role role)
        {
            //given
            var platform = new PlatformModel();
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = role });

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<AuthorizationException>(() => service.Restore(42, 42),
                "Your current role doesn't have permissions to do this.");
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void RestoreIdNotFoundPlatformExceptionTest()
        {
            //given
            var platform = new PlatformModel { Station = new StationModel { Id = 1 } };
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User() { Role = Role.Admin });
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Platform?)null);

            var service = new PlatformService(_mapper, _platformRepositoryMock.Object, _userRepositoryMock.Object,
                _stationRepositoryMock.Object);

            //when then
            Assert.Throws<NotFoundException>(() => service.Restore(42, 42));
            _userRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
            _platformRepositoryMock.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        }
    }
}
