using System;
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
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.BusinessLayer.Tests
{
    public class PlatformMaintenanceServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPlatformMaintenanceRepository> _platformMaintenanceRepositoryMock;
        private Mock<IPlatformRepository> _platformRepositoryMock;
        private PlatformMaintenanceService _service;
        private readonly IMapper _mapper;

        public PlatformMaintenanceServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _platformMaintenanceRepositoryMock = new Mock<IPlatformMaintenanceRepository>();
            _platformRepositoryMock = new Mock<IPlatformRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new PlatformMaintenanceService(_mapper, _userRepositoryMock.Object, _platformMaintenanceRepositoryMock.Object, _platformRepositoryMock.Object);
        }

        // GetList
        [TestCaseSource(typeof(PlatformMaintenanceServiceTestCaseSource), nameof(PlatformMaintenanceServiceTestCaseSource.GetListByIdStationTestCases))]
        public void GetListTest(List<PlatformMaintenance> platformMaintenance, List<PlatformMaintenanceModel> expectedPlatformMaintenanceModels, Role role, int stationId)
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.GetListByStationId(It.IsAny<int>(), false)).Returns(platformMaintenance);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            var platformMaintenanceModels = _service.GetListByStationId(stationId, 10);

            // then
            _userRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(x => x.GetListByStationId((It.IsAny<int>()), false), Times.Once);
            CollectionAssert.AreEqual(expectedPlatformMaintenanceModels, platformMaintenanceModels);
        }

        [Test]
        public void GetListNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetListByStationId(10,10));
        }

        [Test]
        public void GetListNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetListByStationId(10,10));
        }

        // GetListDeleted
        [TestCaseSource(typeof(PlatformMaintenanceServiceTestCaseSource), nameof(PlatformMaintenanceServiceTestCaseSource.GetListDeletdByIdStationTestCases))]
        public void GetListDeletedTest(List<PlatformMaintenance> platformMaintenance, List<PlatformMaintenanceModel> expectedPlatformMaintenanceModels, Role role,int stationId)
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.GetListByStationId(stationId, true)).Returns(platformMaintenance);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            var platformMaintenanceModels = _service.GetListDeletedByStationId(stationId, 10);

            // then
            _userRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(x => x.GetListByStationId((It.IsAny<int>()), true), Times.Once);
            CollectionAssert.AreEqual(expectedPlatformMaintenanceModels, platformMaintenanceModels);
        }
    

        [Test]
        public void GetListDeletedNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetListDeletedByStationId(10, 10));
        }

        [Test]
        public void GetListDeletedNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.StationManager });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetListDeletedByStationId(10,10));
        }

        // GetById
        [TestCaseSource(typeof(PlatformMaintenanceServiceTestCaseSource), nameof(PlatformMaintenanceServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(PlatformMaintenance platformMaintenance, PlatformMaintenanceModel expectedPlatformMaintenance, Role role)
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platformMaintenance);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });


            // when
            var actual = _service.GetById(10,10);

            // then
            Assert.AreEqual(expectedPlatformMaintenance, actual);
            _platformMaintenanceRepositoryMock.Verify(pm => pm.GetById(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void GetByIdNegativeTest()
        {
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((PlatformMaintenance?)null);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            Assert.Throws<NotFoundException>(() => _service.GetById(10 ,42));
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetByIdNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetById(10, 10));
        }

        //Add
        [TestCase(10, Role.Admin)]
        [TestCase(10, Role.StationManager)]
        public void AddTest(int expected, Role role)
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.Add(It.IsAny<PlatformMaintenance>())).Returns(expected);
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Platform());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });
            // when
            int actual = _service.Add(10, new PlatformMaintenanceModel
            {
                Platform = new PlatformModel { Id = 1 }
            });

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.Add(It.IsAny<PlatformMaintenance>()), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.Add(10, new PlatformMaintenanceModel()));
        }

        [Test]
        public void AddNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Add(10, new PlatformMaintenanceModel()));
        }

        //Update
        [TestCase(Role.Admin)]
        [TestCase(Role.StationManager)]
        public void UpdateTest(Role role)
        {
            
            // given
            var platformMaintenance = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), It.IsAny<PlatformMaintenance>()));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platformMaintenance);
            _platformRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Platform());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });



            // when
            _service.Update(1, 1, new PlatformMaintenanceModel
            {
                Platform = new PlatformModel
                {
                    Number = 111,
                    Station = new StationModel
                    {
                        Name = "aaa",
                        Platforms = new List<PlatformModel>
                            {
                                new()
                                {
                                    Number=11
                                },
                                new()
                                {
                                    Number=22
                                }
                            }
                    }
                },
                StartTime = new DateTime(2000, 1, 1),
                EndTime = new DateTime(2001, 1, 1),
                IsDeleted = false
            });

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(platformMaintenance, It.IsAny<PlatformMaintenance>()), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void UpdateNegativeTest()
        {
            // given
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), It.IsAny<PlatformMaintenance>()));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((PlatformMaintenance?)null);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });


            // when then
            Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new PlatformMaintenanceModel()));
        }

        [Test]
        public void UpdateNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.Update(10, 10, new PlatformMaintenanceModel()));
        }

        [Test]
        public void UpdateNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new PlatformMaintenanceModel()));
        }


        //Delete
        [TestCase(Role.Admin)]
        [TestCase(Role.StationManager)]
        public void DeleteTest(Role role)
        {
            // given
            var platformMaintenance = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), true));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platformMaintenance);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            _service.Delete(10, 10);

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(platformMaintenance, true), Times.Once);
        }

        [Test]
        public void DeleteNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            // when then
            Assert.Throws<NotFoundException>(() => _service.Delete(10, 10));
        }

        [Test]
        public void DeleteNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Delete(10, 10));
        }

        //Restore
        [TestCase(Role.Admin)]
        public void RestoreTest(Role role)
        {
            // given
            var platformMaintenance = new PlatformMaintenance();
            _platformMaintenanceRepositoryMock.Setup(x => x.Update(It.IsAny<PlatformMaintenance>(), true));
            _platformMaintenanceRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(platformMaintenance);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });


            // when
            _service.Restore(10, 10);

            // then
            _platformMaintenanceRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _platformMaintenanceRepositoryMock.Verify(s => s.Update(platformMaintenance, false), Times.Once);
        }

        [Test]
        public void RestoreNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            // when then
            Assert.Throws<NotFoundException>(() => _service.Restore(10, 10));
        }

        [Test]
        public void RestoreNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.StationManager });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Restore(10, 10));
        }

    }
}
