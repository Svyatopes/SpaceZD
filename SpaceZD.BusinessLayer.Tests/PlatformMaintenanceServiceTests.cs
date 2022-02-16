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
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Tests
{
    public class PlatformMaintenanceServiceTests
    {
        private Mock<IRepositorySoftDelete<PlatformMaintenance>> _platformMaintenanceRepositoryMock;
        private readonly IMapper _mapper;

        public PlatformMaintenanceServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BusinessLayerMapper>()));
        }

        [SetUp]
        public void SetUp()
        {
            _platformMaintenanceRepositoryMock = new Mock<IRepositorySoftDelete<PlatformMaintenance>>();
        }

    }
}
