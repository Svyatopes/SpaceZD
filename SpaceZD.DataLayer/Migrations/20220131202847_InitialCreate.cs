using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceZD.DataLayer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarriageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NumberOfSeats = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarriageType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Passport = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Station",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Station", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Train",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Train", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Platform_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Route",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartStationId = table.Column<int>(type: "int", nullable: false),
                    EndStationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Route", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Route_Station_EndStationId",
                        column: x => x.EndStationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Route_Station_StartStationId",
                        column: x => x.StartStationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartStationId = table.Column<int>(type: "int", nullable: false),
                    EndStationId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transit_Station_EndStationId",
                        column: x => x.EndStationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transit_Station_StartStationId",
                        column: x => x.StartStationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Carriage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TrainId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carriage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carriage_CarriageType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CarriageType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Carriage_Train_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Train",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NotWorkPlatform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotWorkPlatform", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotWorkPlatform_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainId = table.Column<int>(type: "int", nullable: false),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trip_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trip_Train_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Train",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RouteTransit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransitId = table.Column<int>(type: "int", nullable: false),
                    DepartingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    ArrivalTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RouteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteTransit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteTransit_Route_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Route",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RouteTransit_Transit_TransitId",
                        column: x => x.TransitId,
                        principalTable: "Transit",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TripStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationId = table.Column<int>(type: "int", nullable: false),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TripStation_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripStation_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TripStation_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    StartStationId = table.Column<int>(type: "int", nullable: false),
                    EndStationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_TripStation_EndStationId",
                        column: x => x.EndStationId,
                        principalTable: "TripStation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_TripStation_StartStationId",
                        column: x => x.StartStationId,
                        principalTable: "TripStation",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CarriageId = table.Column<int>(type: "int", nullable: false),
                    SeatNumber = table.Column<int>(type: "int", nullable: false),
                    IsTeaIncluded = table.Column<bool>(type: "bit", nullable: false),
                    IsPetPlaceIncluded = table.Column<bool>(type: "bit", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(9,2)", precision: 9, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ticket_Carriage_CarriageId",
                        column: x => x.CarriageId,
                        principalTable: "Carriage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ticket_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ticket_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carriage_TrainId",
                table: "Carriage",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_Carriage_TypeId",
                table: "Carriage",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotWorkPlatform_PlatformId",
                table: "NotWorkPlatform",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_EndStationId",
                table: "Order",
                column: "EndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StartStationId",
                table: "Order",
                column: "StartStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TripId",
                table: "Order",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_UserId",
                table: "Order",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Platform_StationId",
                table: "Platform",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_EndStationId",
                table: "Route",
                column: "EndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Route_StartStationId",
                table: "Route",
                column: "StartStationId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteTransit_RouteId",
                table: "RouteTransit",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteTransit_TransitId",
                table: "RouteTransit",
                column: "TransitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_CarriageId",
                table: "Ticket",
                column: "CarriageId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_OrderId",
                table: "Ticket",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_PersonId",
                table: "Ticket",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Transit_EndStationId",
                table: "Transit",
                column: "EndStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transit_StartStationId",
                table: "Transit",
                column: "StartStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_RouteId",
                table: "Trip",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_TrainId",
                table: "Trip",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStation_PlatformId",
                table: "TripStation",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStation_StationId",
                table: "TripStation",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_TripStation_TripId",
                table: "TripStation",
                column: "TripId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotWorkPlatform");

            migrationBuilder.DropTable(
                name: "RouteTransit");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Transit");

            migrationBuilder.DropTable(
                name: "Carriage");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "CarriageType");

            migrationBuilder.DropTable(
                name: "TripStation");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropTable(
                name: "Route");

            migrationBuilder.DropTable(
                name: "Train");

            migrationBuilder.DropTable(
                name: "Station");
        }
    }
}
