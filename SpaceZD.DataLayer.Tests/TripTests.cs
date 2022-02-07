using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;

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
        }


        [Test]
        public void GetByIdTest()
        {
            // given
            var tripToAdd = GetTestTrip();

            _context.Trips.Add(tripToAdd);
            _context.SaveChanges();
            var idAddedTrip = tripToAdd.Id;

            // when
            var receivedTrip = _repository.GetById(idAddedTrip);

            // then
            Assert.IsNotNull(receivedTrip);
            Assert.IsFalse(receivedTrip!.IsDeleted);
            AssertTestTrip(receivedTrip);
        }



        [Test]
        public void GetListTest()
        {
            // given
            var tripToAdd = GetTestTrip();
            var secondTripToAdd = GetTestTrip();
            var thirdTripToAdd = GetTestTrip();
            thirdTripToAdd.IsDeleted = true;

            _context.Trips.Add(tripToAdd);
            _context.Trips.Add(secondTripToAdd);
            _context.Trips.Add(thirdTripToAdd);
            _context.SaveChanges();

            // when
            var trips = (List<Trip>)_repository.GetList();

            // then

            Assert.IsNotNull(trips);
            Assert.AreEqual(2, trips.Count);

            var transitToCheck = trips[0];
            Assert.IsNotNull(transitToCheck);
            Assert.IsFalse(transitToCheck.IsDeleted);
            AssertTestTrip(transitToCheck);
        }


        [Test]
        public void GetListAllIncludedTest()
        {
            // given
            var tripToAdd = GetTestTrip();
            var secondTripToAdd = GetTestTrip();
            var thirdTripToAdd = GetTestTrip();
            thirdTripToAdd.IsDeleted = true;

            _context.Trips.Add(tripToAdd);
            _context.Trips.Add(secondTripToAdd);
            _context.Trips.Add(thirdTripToAdd);
            _context.SaveChanges();

            // when
            var trips = (List<Trip>)_repository.GetList(true);


            // then
            Assert.IsNotNull(trips);
            Assert.AreEqual(3, trips.Count);

            var tripToCheck = trips[2];
            Assert.IsNotNull(tripToCheck);
            Assert.IsTrue(tripToCheck.IsDeleted);
            AssertTestTrip(tripToCheck);
        }

        [Test]
        public void AddTest()
        {
            // given
            var tripToAdd = GetTestTrip();

            // when 
            int id = _repository.Add(tripToAdd);

            // then
            var createdTrip = _context.Trips.FirstOrDefault(o => o.Id == id);

            AssertTestTrip(createdTrip!);
        }

        [Test]
        public void UpdateEntityTest()
        {
            // given
            var tripToAdd = GetTestTrip();
            _context.Trips.Add(tripToAdd);
            _context.SaveChanges();

            var tripToEdit = GetTestTrip();
            tripToEdit.Id = tripToAdd.Id;

            //tripToEdit.StartStation = new Station() { Name = "Sevastopol" };
            //tripToEdit.EndStation = new Station() { Name = "Mariupol" };
            //tripToEdit.Price = (decimal?)24.4;

            // when 
            bool edited = _repository.Update(tripToEdit);

            // then
            var updatedTrip = _context.Trips.FirstOrDefault(o => o.Id == tripToEdit.Id);

            Assert.IsTrue(edited);
            Assert.IsNotNull(updatedTrip);
            //Assert.IsNotNull(updatedTrip!.StartStation);
            //Assert.AreEqual("Sevastopol", updatedTrip.StartStation.Name);
            //Assert.IsNotNull(updatedTrip.EndStation);
            //Assert.AreEqual("Bolotnoe", updatedTrip.EndStation.Name);
            //Assert.IsNotNull(updatedTrip.Price);
            //Assert.AreEqual((decimal?)24.4, updatedTrip.Price);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void UpdateIsDeletedTest(bool isDeleted)
        {
            // given
            var tripToEdit = GetTestTrip();
            tripToEdit.IsDeleted = !isDeleted;
            _context.Trips.Add(tripToEdit);
            _context.SaveChanges();

            // when 
            bool edited = _repository.Update(tripToEdit.Id, isDeleted);

            // then
            var updatedTrip = _context.Trips.FirstOrDefault(o => o.Id == tripToEdit.Id);

            Assert.IsTrue(edited);
            Assert.IsNotNull(updatedTrip);
            Assert.AreEqual(isDeleted, updatedTrip!.IsDeleted);
            AssertTestTrip(updatedTrip);
        }

        private Trip GetTestTrip() => new Trip()
        {
            Train = new Train { Id = 3 },
            Route = new Route 
            {
                Code = "M456",
                Transits = new List<RouteTransit>
                {
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "Москва" },
                            EndStation = new Station { Name = "Псков" }
                        },
                        DepartingTime = new TimeSpan(1, 1, 0),
                        ArrivalTime = new TimeSpan(1, 0, 0)
                    },
                    new()
                    {
                        Transit = new Transit
                        {
                            StartStation = new Station { Name = "Псков" },
                            EndStation = new Station { Name = "Новгород" }
                        },
                        DepartingTime = new TimeSpan(2, 1, 0),
                        ArrivalTime = new TimeSpan(2, 0, 0)
                    }
                },
                StartTime = new DateTime(1999, 10, 1),
                StartStation = new Station { Name = "Москва" },
                EndStation = new Station { Name = "Новгород" }
            },
            Stations = new List<TripStation>()
            {

            }

        };

        private void AssertTestTrip(Trip trip)
        {
            Assert.IsNotNull(trip);
           // Assert.IsNotNull(trip.StartStation);
           // Assert.AreEqual("Novosibirsk", trip.StartStation.Name);
         //   Assert.IsNotNull(trip.EndStation);
           // Assert.AreEqual("Sheregesh", trip.EndStation.Name);
          //  Assert.IsNotNull(trip.Price);
           // Assert.AreEqual((decimal?)23.4, trip.Price);
        }
    }
}
