using NUnit.Framework;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using System.Collections.Generic;

namespace SpaceZD.BusinessLayer.Tests.TestCaseSources
{
    public class TicketServiceTestCaseSource
    {
        public static IEnumerable<TestCaseData> GetListTestCases()
        {
            yield return new TestCaseData(GetTicket(), GetTicketModel(), false, 1);
            yield return new TestCaseData(GetTicket(), GetTicketModel(), true, 2);
        }

        public static IEnumerable<TestCaseData> GetByIdTestCases()
        {
            var tickets = GetTicket();
            var ticketModels = GetTicketModel();
            yield return new TestCaseData(tickets[0], ticketModels[0], 1);
            yield return new TestCaseData(tickets[1], ticketModels[1], 2);
        }


        private static List<Ticket> GetTicket()
        {
            var listTicket = new List<Ticket>()
            {
                new Ticket
                {
                    SeatNumber = 45,
                    Carriage = new Carriage()
                    {
                        Number = 34                        
                    },
                    Person = new Person()
                    {
                        FirstName = "Petya",
                        LastName = "Petrov",
                        Patronymic = "Petrovich",
                        Passport = "4567890"
                    },
                    Order = new Order()
                    {
                        StartStation = new TripStation()
                        {
                            Station = new Station { Name  = "Moscow"}
                        },
                        
                    },
                    Price = 798,
                    IsDeleted = false
                },
                new Ticket
                {
                    SeatNumber = 8,
                    Carriage = new Carriage()
                    {
                        Number = 16,
                        Train = new Train()
                        {
                            Carriages = new List<Carriage>()
                        }
                    },
                    Person = new Person()
                    {
                        FirstName = "Vasya",
                        LastName = "Vastrov",
                        Patronymic = "Vastrovich",
                        Passport = "9876543"
                    },
                    Order = new Order()
                    {
                        StartStation = new TripStation()
                        {
                            Station = new Station { Name  = "SPb"}
                        }
                    },
                    Price = 367,
                    IsDeleted = true
                }
            };
            return listTicket;
        }

        private static List<TicketModel> GetTicketModel()
        {
            var listTicketModel = new List<TicketModel>()
            {
                new TicketModel
                {
                    SeatNumber = 45,
                    Carriage = new CarriageModel()
                    {
                        Number = 34,
                        Train = new TrainModel()
                        {
                            Carriages = new List<CarriageModel>()
                        }
                    },
                    Person = new PersonModel()
                    {
                        FirstName = "Petya",
                        LastName = "Petrov",
                        Patronymic = "Petrovich",
                        Passport = "4567890"
                    },

                    Order = new OrderModel()
                    {
                        StartStation = new TripStationModel()
                        {
                            Station = new StationModel { Name = "Moscow"}
                        },

                    },
                    Price = 798,
                    IsDeleted = false
                },
                new TicketModel
                {
                    SeatNumber = 8,
                    Carriage = new CarriageModel()
                    {
                        Number = 16,
                        Train = new TrainModel()
                        {
                            Carriages = new List<CarriageModel>()
                        }
                    },
                    Person = new PersonModel()
                    {
                        FirstName = "Vasya",
                        LastName = "Vastrov",
                        Patronymic = "Vastrovich",
                        Passport = "9876543"
                    },
                    Order = new OrderModel()
                    {
                        StartStation = new TripStationModel()
                        {
                            Station = new StationModel { Name = "SPb"}
                        },

                    },
                    Price = 367,
                    IsDeleted = true
                }
            };
            return listTicketModel;
        }        
    }
}
