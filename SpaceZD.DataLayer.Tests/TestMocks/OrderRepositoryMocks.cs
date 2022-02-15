using System.Collections.Generic;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Tests.TestMocks
{
    public class OrderRepositoryMocks
    {
        public static List<Order> GetOrders() => new List<Order>
        {
            new Order()
            {
                StartStation = new TripStation()
                {
                    Station = new Station() { Name = "Novosibirsk" }
                },
                EndStation = new TripStation()
                {
                    Station = new Station() { Name = "Sheregesh" }
                },
                Trip = new Trip() { Route = new Route() { Code = "231A" } },
                User = new User()
                {
                    Name = "Zagit",
                    Login = "Sleep2000",
                    PasswordHash = "SDJKLSAJDLJASLDJASLDKJASD",
                    Role = Enums.Role.StationManager
                },
                IsDeleted = true
            },

            new Order()
            {
                StartStation = new TripStation()
                {
                    Station = new Station() { Name = "Novosibirsk" }
                },
                Trip = new Trip() { Route = new Route() { Code = "231A" } },
                User = new User()
                {
                    Name = "Zagit",
                    Login = "Sleep2000",
                    PasswordHash = "SDJKLSAJDLJASLDJASLDKJASD",
                    Role = Enums.Role.StationManager
                }
            },

            new Order()
            {
                EndStation = new TripStation()
                {
                    Station = new Station() { Name = "Sheregesh" }
                },
                User = new User()
                {
                    Name = "Jora",
                    Login = "Sleep2001",
                    PasswordHash = "SDSDSD",
                    Role = Enums.Role.StationManager
                }
            },
        };


        public static Order GetOrder() => new Order()
        {
            StartStation = new TripStation()
            {
                Station = new Station() { Name = "Novosibirsk" }
            },
            EndStation = new TripStation()
            {
                Station = new Station() { Name = "Sheregesh" }
            },
            Trip = new Trip() { Route = new Route() { Code = "231A" } },
            User = new User()
            {
                Name = "Zagit",
                Login = "Sleep2000",
                PasswordHash = "SDJKLSAJDLJASLDJASLDKJASD",
                Role = Enums.Role.StationManager
            },
            IsDeleted = true
        };
        

    }
}