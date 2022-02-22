using System.Collections.Generic;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestMocks
{
    public class PlatformRepositoryMocks
    {
        public static List<Platform> GetPlatforms() => new List<Platform>
        {
            new Platform
            {
                Number = 1,
                Station = new Station
                {
                    Name = "NewYork"
                }
                
            },
            new Platform
            {
                Number = 3,
                Station = new Station
                {
                    Name = "Vladivostok"
                }
            },
            new Platform
            {
                Number = 2,
                IsDeleted = true
            },
            new Platform
            {
                Number = 3,
                Station = new Station
                {
                    Name = "Vladivostok",
                    IsDeleted = true
                }
            },

        };


        public static Platform GetPlatform() => new Platform()
        {
            Number = 10,
            Station = new Station
            {
                Name = "New Uganda"
            }
        };


    }
}