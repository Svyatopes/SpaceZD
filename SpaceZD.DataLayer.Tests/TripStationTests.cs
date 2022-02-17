using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestCaseSources;
using System.Linq;

namespace SpaceZD.DataLayer.Tests
{
    public class TripStationTests
    {
        private VeryVeryImportantContext _context;
        private TripStationRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                          .UseInMemoryDatabase(databaseName: "Test")
                          .Options;

            _context = new VeryVeryImportantContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new TripStationRepository(_context);

            // seed
            var tripStations = TripStationRepositoryMocks.GetTripStations();
            _context.TripStations.AddRange(tripStations);
            _context.SaveChanges();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetByIdTest(int id)
        {
            // given
            var expectedEntity = _context.TripStations.Find(id);

            // when
            var receivedEntity = _repository.GetById(id);

            // then
            Assert.AreEqual(expectedEntity, receivedEntity);
        }


        [Test]
        public void GetListTest()
        {
            // given
            var expected = _context.TripStations.ToList();

            // when
            var list = _repository.GetList();

            // then
            CollectionAssert.AreEqual(expected, list);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void UpdateEntityTest(int id)
        {
            // given
            var entityToEdit = _context.TripStations.FirstOrDefault(o => o.Id == id);
            var newEntity = TripStationRepositoryMocks.GetTripStation();

            // when 
            _repository.Update(entityToEdit!, newEntity);

            // then
            var entityUpdated = _context.TripStations.FirstOrDefault(o => o.Id == entityToEdit.Id);
            Assert.AreEqual(entityToEdit, entityUpdated);
        }
    }
}
