using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal class PlatformMaintenanceServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetPlatformMaintenance(), ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenance()), Role.Admin);
            yield return new TestCaseData(GetPlatformMaintenance(), ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenance()), Role.StationManager);
        }

        public static IEnumerable<TestCaseData> GetListDeletdTestCases()
        {
            yield return new TestCaseData(GetPlatformMaintenance(), ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenanceDeleted()), Role.Admin);
        }

        public static IEnumerable<TestCaseData> GetListDeleteTestCases()
        {
            yield return new TestCaseData(GetPlatformMaintenance(), ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenance()), false);
            yield return new TestCaseData(GetPlatformMaintenance(), ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenance()), true);
        }
        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var platformsMaintenance = GetPlatformMaintenance();
            yield return new TestCaseData(platformsMaintenance[0], ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenance())[0]);
            yield return new TestCaseData(platformsMaintenance[1], ConvertPlatformMaintenanceToPlatformMaintenanceModels(GetPlatformMaintenance())[1]);
        }

        private static List<PlatformMaintenance> GetPlatformMaintenance() => new List<PlatformMaintenance>
        {
                new PlatformMaintenance
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
                    EndTime= new DateTime(2001,1,1),
                    IsDeleted= false
                },
                new PlatformMaintenance
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
                    EndTime= new DateTime(2010,10,1),
                    IsDeleted= false
                },
                new PlatformMaintenance
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

        private static List<PlatformMaintenance> GetPlatformMaintenanceDeleted() => new List<PlatformMaintenance>
        {
            new PlatformMaintenance
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

        private static List<PlatformMaintenanceModel> ConvertPlatformMaintenanceToPlatformMaintenanceModels(List<PlatformMaintenance> platformMaintenance, bool includeAll = true)
        {
            return platformMaintenance
                .Where(pm => includeAll || pm.IsDeleted)
                .Select(platformMaintenance => new PlatformMaintenanceModel
                {
                    Id=platformMaintenance.Id,
                    Platform = new PlatformModel
                    {
                        Id=platformMaintenance.Platform.Id,
                        Number = platformMaintenance.Platform.Number,
                        IsDeleted = platformMaintenance.Platform.IsDeleted,
                        Station = new StationModel { Id=platformMaintenance.Platform.Station.Id, Name = platformMaintenance.Platform.Station.Name, IsDeleted = platformMaintenance.Platform.Station.IsDeleted }
                    },
                    StartTime = platformMaintenance.StartTime,
                    EndTime = platformMaintenance.EndTime,
                    IsDeleted = platformMaintenance.IsDeleted
                }).ToList();
        }
    }
}
