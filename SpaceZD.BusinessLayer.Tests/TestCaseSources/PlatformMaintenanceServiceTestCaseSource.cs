using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    internal class PlatformMaintenanceServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetPlatformMaintenance(), GetPlatformMaintenanceModel(), false);
            yield return new TestCaseData(GetPlatformMaintenance(), GetPlatformMaintenanceModel(), true);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var platformsMaintenance = GetPlatformMaintenance();
            var platformsMaintenanceModels = GetPlatformMaintenanceModel();
            yield return new TestCaseData(platformsMaintenance[0], platformsMaintenanceModels[0]);
            yield return new TestCaseData(platformsMaintenance[1], platformsMaintenanceModels[1]);
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

        private static List<PlatformMaintenanceModel> GetPlatformMaintenanceModel() => new List<PlatformMaintenanceModel>
        {
                new PlatformMaintenanceModel
                {
                    Platform= new PlatformModel
                    {
                        Number= 1,
                        Station= new StationModel
                        {
                            Name="СПБ",
                            Platforms=new List<PlatformModel>
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
                new PlatformMaintenanceModel
                {
                    Platform= new PlatformModel
                    {
                        Number= 2,
                        Station= new StationModel
                        {
                            Name="Москва",
                            Platforms=new List<PlatformModel>
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
                new PlatformMaintenanceModel
                {
                    Platform= new PlatformModel
                    {
                        Number= 3,
                        Station= new StationModel
                        {
                            Name="Пермь",
                            Platforms=new List<PlatformModel>
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
    }
}
