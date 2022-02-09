using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Repositories;
using System;
using System.Collections.Generic;
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

            // seed
            var trips = new Trip[]
                {
                new()
                {
                    Train = new Train()
                    {
                        Id = 1,
                        IsDeleted = false
                    },
                    Route = new Route()
                    {
                         Id = 1,
                         Code = "F789",
                         Transits = new List<RouteTransit>
                         {
                            new()
                            {
                            Transit = new Transit
                                {
                                    StartStation = new Station { Name = "С-Пб" },
                                    EndStation = new Station { Name = "Выборг" }
                                },
                            DepartingTime = new TimeSpan(0, 0, 1),
                            ArrivalTime = new TimeSpan(2, 30, 0)
                            }
                         },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    IsDeleted = false
                },
                new()
                {
                    Train = new Train()
                    {
                        Id = 2,
                        IsDeleted = false
                    },
                    Route = new Route()
                    {
                         Id = 2,
                         Code = "F799",
                         Transits = new List<RouteTransit>
                         {
                            new()
                            {
                            Transit = new Transit
                                {
                                    StartStation = new Station { Name = "С-Пб" },
                                    EndStation = new Station { Name = "Выборг" }
                                },
                            DepartingTime = new TimeSpan(0, 0, 1),
                            ArrivalTime = new TimeSpan(2, 30, 0)
                            }
                         },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    IsDeleted = false
                },
                new()
                {
                    Train = new Train()
                    {
                        Id = 3,
                        IsDeleted = false
                    },
                    Route = new Route()
                    {
                         Id = 3,
                         Code = "F710",
                         Transits = new List<RouteTransit>
                         {
                            new()
                            {
                            Transit = new Transit
                                {
                                    StartStation = new Station { Name = "С-Пб" },
                                    EndStation = new Station { Name = "Выборг" }
                                },
                            DepartingTime = new TimeSpan(0, 0, 1),
                            ArrivalTime = new TimeSpan(2, 30, 0)
                            }
                         },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    StartStation = new Station { Name = "С-Пб" },
                    EndStation = new Station { Name = "Выборг" }
                    },
                    StartTime = new DateTime(1970, 1, 1, 5, 30, 0),
                    IsDeleted = false
                }
            };
            _context.Trips.AddRange(trips);
            _context.SaveChanges();
        }
        public virtual Train Train { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<TripStation> Stations { get; set; }
        public DateTime StartTime { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public bool IsDeleted { get; set; }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetByIdTest(int id)
        {
            // given
            var expectedEntity = _context.Trips.Find(id);

            // when
            var receivedEntity = _repository.GetById(id);

            // then
            Assert.AreEqual(expectedEntity, receivedEntity);
        }


        [Test]
        public void GetListTest()
        {
            // given
            var expected = _context.Trips.ToList();

            // when
            var list = _repository.GetList();

            // then
            CollectionAssert.AreEqual(expected, list);
        }


        [Test]
        public void AddTest()
        {
            // given
            var entityToAdd = TestEntity();

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
            //entityToEdit!.Station . Name = "Oredez";

            // when 
            bool edited = _repository.Update(entityToEdit);

            // then
            var entityUpdated = _context.TripStations.FirstOrDefault(o => o.Id == entityToEdit.Id);

            Assert.IsTrue(edited);
            Assert.AreEqual(entityToEdit, entityUpdated);
        }


        private Trip TestEntity() => new Trip()
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
    }
}