using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal class PlatformServiceTestCaseSource
    {

        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetPlatformModels().Where(pm => !pm.IsDeleted).ToList(),
                GetPlatforms().Where(p => !p.IsDeleted).ToList(), Role.StationManager);
            yield return new TestCaseData(GetPlatformModels(), GetPlatforms(), Role.Admin);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            yield return new TestCaseData(GetPlatformModels()[0], GetPlatforms()[0], Role.StationManager);
            yield return new TestCaseData(GetPlatformModels()[0], GetPlatforms()[0], Role.Admin);
        }

        public static IEnumerable<TestCaseData> AddTestCases()
        {
            yield return new TestCaseData(GetPlatformModels()[0], Role.StationManager);
            yield return new TestCaseData(GetPlatformModels()[0], Role.Admin);
        }

        public static IEnumerable<TestCaseData> EditTestCases()
        {
            yield return new TestCaseData(GetPlatformModels()[0], GetPlatforms()[0], Role.StationManager);
            yield return new TestCaseData(GetPlatformModels()[0], GetPlatforms()[0], Role.Admin);
        }

        public static IEnumerable<TestCaseData> DeleteTestCases()
        {
            yield return new TestCaseData(GetPlatforms()[0], Role.StationManager);
            yield return new TestCaseData(GetPlatforms()[0], Role.Admin);
        }

        public static IEnumerable<TestCaseData> RestoreTestCases()
        {
            yield return new TestCaseData(GetPlatforms()[0], Role.Admin);
        }

        private static List<Platform> GetPlatforms()
        {
            var stationSpb = new Station { Name = "SPb", Id = 1 };
            var platforms = new List<Platform>()
            {
                new Platform { Number = 1, Station = stationSpb},
                new Platform { Number = 2, Station = stationSpb},
                new Platform { Number = 3, Station = stationSpb, IsDeleted = true},
                new Platform { Number = 4, Station = stationSpb}
            };
            return platforms;
        }

        private static List<PlatformModel> GetPlatformModels()
        {
            var stationSpb = new StationModel { Name = "SPb", Id = 1 };
            var platforms = new List<PlatformModel>()
            {
                new PlatformModel { Number = 1, Station = stationSpb},
                new PlatformModel { Number = 2, Station = stationSpb},
                new PlatformModel { Number = 3, Station = stationSpb, IsDeleted = true},
                new PlatformModel { Number = 4, Station = stationSpb}
            };
            return platforms;
        }
    }
}
