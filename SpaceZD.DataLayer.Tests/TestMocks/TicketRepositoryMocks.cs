using SpaceZD.DataLayer.Entities;
using System.Collections.Generic;

namespace SpaceZD.DataLayer.Tests.TestMocks
{
    public class TicketRepositoryMocks
    {
        public static Ticket GetTestEntity() => new Ticket()
        {
            Person = new Person()
            {
                FirstName = "Vasya",
                Patronymic = "Vasilevich",
                LastName = "Vasilev",
                Passport = "55585885999"
            },
            SeatNumber = 56,
            Carriage = new Carriage()
            {
                Number = 5,
                Type = new CarriageType()
                {
                    Name = "Econom",
                    NumberOfSeats = 67
                }
            },
            Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },
            Price = 345
        };

        public static List<Ticket> GetListTestEntities() => new List<Ticket>()
        {
            new()
            {
                Id = 1,
                Person = new Person()
                {
                    FirstName = "Vasya",
                    Patronymic = "Vasilevich",
                    LastName = "Vasilev",
                    Passport = "55585885999"
                },
                SeatNumber = 56,
                Carriage = new Carriage()
                {
                    Number = 5,
                    Type = new CarriageType()
                    {
                        Name = "Econom",
                        NumberOfSeats = 67
                    }
                },
                Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },
                Price = 345

            },
            new()
            {
                Person = new Person()
                {
                    FirstName = "Masha",
                    Patronymic = "Mashevna",
                    LastName = "Mashech",
                    Passport = "5784930"
                },
                SeatNumber = 6,
                Carriage = new Carriage()
                {
                    Number = 5,
                    Type = new CarriageType()
                    {
                        Name = "Platskart",
                        NumberOfSeats = 50
                    }
                },
                Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },

                Price = 300
            },
            new()
            {
                Person = new Person()
                {
                    FirstName = "Dima",
                    Patronymic = "Dmitrievich",
                    LastName = "Dmitr",
                    Passport = "6583939585"
                },
                SeatNumber = 3,
                Carriage = new Carriage()
                {
                    Number = 9,
                    Type = new CarriageType()
                    {
                        Name = "Bisuness",
                        NumberOfSeats = 20
                    }
                },
                Order = new Order() { StartStation = new TripStation() { ArrivalTime = new System.DateTime() } },

                Price = 555
            }
        };
    }
}
