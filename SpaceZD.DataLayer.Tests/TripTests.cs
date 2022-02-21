using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Repositories;
using SpaceZD.DataLayer.Tests.TestCaseSources;
using System.Linq;

namespace SpaceZD.DataLayer.Tests
{
    public class TripTests
    {
        private VeryVeryImportantContext _context;
        private TripRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                         .UseInMemoryDatabase(databaseName: "Test")
                         .Options;

            _context = new VeryVeryImportantContext(options);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new TripRepository(_context);

            var trips = TripRepositoryMocks.GetTrips();
            _context.Trips.AddRange(trips);
            _context.SaveChanges();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetByIdTest(int id)
        {
            // given
            var expected = _context.Trips.Find(id);

            // when
            var actual = _repository.GetById(id);

            // then
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void GetListTest()
        {
            // given
            var expected = TripRepositoryMocks.GetTrips();

            // when
            var actual = _repository.GetList();

            // then
            CollectionAssert.AreEqual(expected, actual);
        }


        [Test]
        public void AddTest()
        {
            // given
            var entityToAdd = TripRepositoryMocks.GetTrip();

            // when 
            int id = _repository.Add(entityToAdd);

            // then
            var entityOnCreate = _context.Trips.FirstOrDefault(o => o.Id == id);

            Assert.AreEqual(entityOnCreate, entityToAdd);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void UpdateEntityTest(int id)
        {
            // given
            var entityToEdit = _context.Trips.FirstOrDefault(o => o.Id == id);
            var newEntity = TripRepositoryMocks.GetTrip();

            // when 
            _repository.Update(entityToEdit!, newEntity);

            // then
            var entityUpdated = _context.Trips.FirstOrDefault(o => o.Id == entityToEdit!.Id);
            Assert.AreEqual(entityToEdit, entityUpdated);
        }


        [TestCase(true)]
        [TestCase(false)]
        public void UpdateIsDeletedTest(bool isDeleted)
        {
            // given
            var entityToEdit = TripRepositoryMocks.GetTrip();
            entityToEdit.IsDeleted = !isDeleted;
            _context.Trips.Add(entityToEdit);
            _context.SaveChanges();

            // when 
            _repository.Update(entityToEdit, isDeleted);

            // then
            var entityToUpdated = _context.Trips.FirstOrDefault(o => o.Id == entityToEdit.Id);

            Assert.AreEqual(entityToEdit, entityToUpdated);
        }
    }
}