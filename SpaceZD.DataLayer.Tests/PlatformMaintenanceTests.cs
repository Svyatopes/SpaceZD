using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.DataLayer.Tests;

public class PlatformMaintenanceTests
{
    private VeryVeryImportantContext _context;
    private IRepositorySoftDelete<PlatformMaintenance> _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<VeryVeryImportantContext>()
                    .UseInMemoryDatabase("Test")
                    .Options;
        _context = new VeryVeryImportantContext(options);
        _repository = new PlatformMaintenanceRepository(_context);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();


        // seed
        var platformsMaintenance = new PlatformMaintenance[]
        {
                new()
                {
                    Platform= new Platform
                    {
                        Number= 1,
                        Station= new Station
                        {
                            Name="СПБ",
                            Platforms=new List<Platform>
                            {
                                new()
                                {
                                    Number=1
                                },
                                new()
                                {
                                    Number=2
                                }
                            }
                        }
                    },
                    StartTime= new DateTime(2000,1,1),
                    EndTime= new DateTime(2001,1,1)
                },
                new()
                {
                    Platform= new Platform
                    {
                        Number= 2,
                        Station= new Station
                        {
                            Name="Москва",
                            Platforms=new List<Platform>
                            {
                                new()
                                {
                                    Number=1
                                },
                                new()
                                {
                                    Number=2
                                }
                            }
                        }
                    },
                    StartTime= new DateTime(2002,1,1),
                    EndTime= new DateTime(2010,10,1)
                },
                new()
                {
                    Platform= new Platform
                    {
                        Number= 3,
                        Station= new Station
                        {
                            Name="Пермь",
                            Platforms=new List<Platform>
                            {
                                new()
                                {
                                    Number=1
                                },
                                new()
                                {
                                    Number=2
                                },
                                new()
                                {
                                    Number=3
                                }
                            }
                        }
                    },
                    StartTime= new DateTime(2000,1,1),
                    EndTime= new DateTime(2000,12,1),
                    IsDeleted=true

                }
        };
        _context.PlatformMaintenances.AddRange(platformsMaintenance);
        _context.SaveChanges();

    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void GetByIdTest(int id)
    {
        // given
        var expected = _context.PlatformMaintenances.Find(id);

        // when
        var received = _repository.GetById(id);

        // then
        Assert.AreEqual(expected, received);
    }

    [TestCase(false)]
    [TestCase(true)]
    public void GetListTest(bool includeAll)
    {
        // given
        var expected = _context.PlatformMaintenances.Where(t => !t.IsDeleted || includeAll).ToList();

        // when
        var list = _repository.GetList(includeAll);

        // then
        CollectionAssert.AreEqual(expected, list);
    }

    [Test]
    public void AddTest()
    {
        // given
        var platformMaintenanceToAdd = GetPlatformMaintenance;

        // when 
        int id = _repository.Add(platformMaintenanceToAdd);

        // then
        var platformMaintenanceOnCreate = _context.PlatformMaintenances.FirstOrDefault(o => o.Id == id);

        Assert.AreEqual(platformMaintenanceOnCreate, platformMaintenanceToAdd);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    public void UpdateRouteTransitTest(int id)
    {
        // given
        var oldPlatformMaintenanceToEdit = _context.PlatformMaintenances.FirstOrDefault(o => o.Id == id);
        var newPlatformMaintenanceToEdit = GetPlatformMaintenance;

        // when 
        _repository.Update(oldPlatformMaintenanceToEdit, newPlatformMaintenanceToEdit);

        // then
        var platformMaintenanceToUpdated = _context.PlatformMaintenances.FirstOrDefault(o => o.Id == oldPlatformMaintenanceToEdit.Id);

        Assert.AreEqual(newPlatformMaintenanceToEdit.Platform, platformMaintenanceToUpdated.Platform);
        Assert.AreEqual(newPlatformMaintenanceToEdit.StartTime, platformMaintenanceToUpdated.StartTime);
        Assert.AreEqual(newPlatformMaintenanceToEdit.EndTime, platformMaintenanceToUpdated.EndTime);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void UpdateIsDeletedTest(bool isDeleted)
    {
        // given
        var platformMaintenanceToEdit = GetPlatformMaintenance;
        platformMaintenanceToEdit.IsDeleted = isDeleted;
        _context.PlatformMaintenances.Add(platformMaintenanceToEdit);
        _context.SaveChanges();

        // when 
        _repository.Update(platformMaintenanceToEdit, isDeleted);

        // then
        Assert.AreEqual(isDeleted, platformMaintenanceToEdit.IsDeleted);
    }

    private PlatformMaintenance GetPlatformMaintenance => new()
    {
        Platform = new()
        {
            Number = 2,
            Station = new Station
            {
                Name = "Выборг",
                Platforms = new List<Platform>
                            {
                                new()
                                {
                                    Number=1
                                },
                                new()
                                {
                                    Number=2
                                }
                            }
            }
        },
        StartTime = new DateTime(2020, 1, 1),
        EndTime = new DateTime(2021, 10, 1)

    };
}

