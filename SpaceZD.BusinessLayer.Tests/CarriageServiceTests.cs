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

namespace SpaceZD.BusinessLayer.Tests
{
    public class CarriageServiceTests
    {
        private Mock<IRepositorySoftDelete<CarriageType>> _carriageTypeRepositoryMock;
        private Mock<IRepositorySoftDelete<Train>> _trainRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IRepositorySoftDelete<Carriage>> _carriageRepositoryMock;
        private ICarriageService _service;
        private readonly IMapper _mapper;

        public CarriageServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _carriageRepositoryMock = new Mock<IRepositorySoftDelete<Carriage>>();
            _carriageTypeRepositoryMock = new Mock<IRepositorySoftDelete<CarriageType>>();
            _trainRepositoryMock = new Mock<IRepositorySoftDelete<Train>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _service = new CarriageService(_mapper, _userRepositoryMock.Object, _carriageRepositoryMock.Object, _carriageTypeRepositoryMock.Object, _trainRepositoryMock.Object);
        }

        // GetList
        [TestCaseSource(typeof(CarriageServiceTestCaseSource), nameof(CarriageServiceTestCaseSource.GetListTestCases))]
        public void GetListTest(List<Carriage> carriage, List<CarriageModel> expectedCarriageModels, Role role)
        {
            // given
            _carriageRepositoryMock.Setup(x => x.GetList(false)).Returns(carriage);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            var actual = _service.GetList(10);

            // then
            _carriageRepositoryMock.Verify(s => s.GetList(false), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            CollectionAssert.AreEqual(expectedCarriageModels, actual);
        }

        [Test]
        public void GetListNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetList(10));
        }

        [Test]
        public void GetListNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetById(10, 10));
        }

        // GetById
        [TestCaseSource(typeof(CarriageServiceTestCaseSource), nameof(CarriageServiceTestCaseSource.GetByIdTestCases))]
        public void GetByIdTest(Carriage carriage, CarriageModel expectedCarriage, Role role)
        {
            // given
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriage);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });


            // when
            var actual = _service.GetById(45, 10);

            // then
            Assert.AreEqual(expectedCarriage, actual);
            _carriageRepositoryMock.Verify(pm => pm.GetById(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void GetByIdNegativeTest()
        {
            // given
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetById(45, 10));
        }

        [Test]
        public void GetByIdNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetById(10, 10));
        }

        [Test]
        public void GetByIdNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetById(10, 10));
        }

        // Add
        [TestCase(45, Role.Admin)]
        [TestCase(45, Role.TrainRouteManager)]
        public void AddTest(int expected, Role role)
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Add(It.IsAny<Carriage>())).Returns(expected);
            _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new CarriageType());
            _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });
            // when
            int actual = _service.Add(45, new CarriageModel
            {
                Type = new CarriageTypeModel { Id = 1 },
                Train = new TrainModel { Id = 2 }
            });

            // then
            
            _carriageRepositoryMock.Verify(s => s.Add(It.IsAny<Carriage>()), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(45), Times.Once);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AddNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Add(10, new CarriageModel()));
        }

        //Update
        [TestCase(Role.Admin)]
        [TestCase(Role.TrainRouteManager)]
        public void UpdateTest(Role role)
        {
            // given
            var carriage = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), It.IsAny<Carriage>()));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriage);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });
            _carriageTypeRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new CarriageType());
            _trainRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Train());

            // when

            _service.Update(2, 2, new CarriageModel
            {
                Type = new CarriageTypeModel { Id = 1, Name="ddd" },
                Train = new TrainModel { Id = 2 }
            });
            // then
            _carriageRepositoryMock.Verify(s => s.GetById(2), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(carriage, It.IsAny<Carriage>()), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(2), Times.Once);
        }

        [Test]
        public void UpdateNegativeTest()
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), It.IsAny<Carriage>()));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            // when then
            Assert.Throws<NotFoundException>(() => _service.Update(45,10, new CarriageModel()));
        }

        [Test]
        public void UpdateNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.User });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Update(10, 10, new CarriageModel()));
        }

        //Delete
        [TestCase(Role.Admin)]
        public void DeleteTest(Role role)
        {
            // given
            var carriage = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), true));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriage);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = role });

            // when
            _service.Delete(10, 10);

            // then
            _carriageRepositoryMock.Verify(s => s.GetById(10), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(carriage, true), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(10), Times.Once);
        }

        [Test]
        public void DeleteNegativeTest()
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), true));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            // when then
            Assert.Throws<NotFoundException>(() => _service.Delete(45,10));  
        }

        [Test]
        public void GetListDeletedNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.GetListDeleted(10));
        }

        [Test]
        public void GetListDeletedNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.TrainRouteManager });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.GetListDeleted(10));
        }

        //Restore
        [Test]
        public void RestoreTest()
        {
            var carriage = new Carriage();
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), false));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(carriage);
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.Admin });

            // when
            _service.Restore(1, 1);

            // then
            _carriageRepositoryMock.Verify(s => s.GetById(1), Times.Once);
            _carriageRepositoryMock.Verify(s => s.Update(carriage, false), Times.Once);
            _userRepositoryMock.Verify(s => s.GetById(1), Times.Once);
        }

        [Test]
        public void RestoreNegativeTest()
        {
            // given
            _carriageRepositoryMock.Setup(x => x.Update(It.IsAny<Carriage>(), false));
            _carriageRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((Carriage?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.Restore(50, 10));
        }

        [Test]
        public void RestoreNegativeNotFoundExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns((User?)null);

            // when then
            Assert.Throws<NotFoundException>(() => _service.Restore(10, 10));
        }

        [Test]
        public void RestoreNegativeAuthorizationExceptionTest()
        {
            // given
            _userRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(new User { Role = Role.TrainRouteManager });

            // when then
            Assert.Throws<AuthorizationException>(() => _service.Restore(10, 10));
        }
    }
}
