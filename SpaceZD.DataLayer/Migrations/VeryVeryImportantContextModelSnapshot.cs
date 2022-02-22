﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpaceZD.DataLayer.DbContextes;

#nullable disable

namespace SpaceZD.DataLayer.Migrations
{
    [DbContext(typeof(VeryVeryImportantContext))]
    partial class VeryVeryImportantContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Carriage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("TrainId")
                        .HasColumnType("int");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrainId");

                    b.HasIndex("TypeId");

                    b.ToTable("Carriage");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.CarriageType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("int");

                    b.Property<double>("PriceFactor")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(6, 3)
                        .HasColumnType("float(6)")
                        .HasDefaultValue(1.0);

                    b.HasKey("Id");

                    b.ToTable("CarriageType");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EndStationId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("StartStationId")
                        .HasColumnType("int");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EndStationId");

                    b.HasIndex("StartStationId");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Passport")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Platform", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.HasIndex("Number", "StationId")
                        .IsUnique();

                    b.ToTable("Platform");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.PlatformMaintenance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("PlatformId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.ToTable("PlatformMaintenance");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("EndStationId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("StartStationId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EndStationId");

                    b.HasIndex("StartStationId");

                    b.ToTable("Route");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.RouteTransit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<TimeSpan>("ArrivalTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("DepartingTime")
                        .HasColumnType("time");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<int>("TransitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("TransitId");

                    b.ToTable("RouteTransit");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Station", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Station");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CarriageId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsPetPlaceIncluded")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTeaIncluded")
                        .HasColumnType("bit");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<int>("SeatNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarriageId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PersonId");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Train", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("Train");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Transit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EndStationId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<decimal?>("Price")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<int>("StartStationId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EndStationId");

                    b.HasIndex("StartStationId", "EndStationId")
                        .IsUnique();

                    b.ToTable("Transit");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("RouteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TrainId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("TrainId");

                    b.ToTable("Trip");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.TripStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DepartingTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PlatformId")
                        .HasColumnType("int");

                    b.Property<int>("StationId")
                        .HasColumnType("int");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlatformId");

                    b.HasIndex("StationId");

                    b.HasIndex("TripId");

                    b.ToTable("TripStation");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Carriage", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Train", "Train")
                        .WithMany("Carriages")
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.CarriageType", "Type")
                        .WithMany("Carriages")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Train");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Order", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.TripStation", "EndStation")
                        .WithMany("OrdersWithEndStation")
                        .HasForeignKey("EndStationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.TripStation", "StartStation")
                        .WithMany("OrdersWithStartStation")
                        .HasForeignKey("StartStationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Trip", "Trip")
                        .WithMany("Orders")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EndStation");

                    b.Navigation("StartStation");

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Person", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.User", "User")
                        .WithMany("Persons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Platform", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Station", "Station")
                        .WithMany("Platforms")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.PlatformMaintenance", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Platform", "Platform")
                        .WithMany("PlatformMaintenances")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Platform");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Route", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Station", "EndStation")
                        .WithMany("RoutesWithEndStation")
                        .HasForeignKey("EndStationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Station", "StartStation")
                        .WithMany("RoutesWithStartStation")
                        .HasForeignKey("StartStationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EndStation");

                    b.Navigation("StartStation");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.RouteTransit", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Route", "Route")
                        .WithMany("RouteTransits")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Transit", "Transit")
                        .WithMany("RouteTransit")
                        .HasForeignKey("TransitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Route");

                    b.Navigation("Transit");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Ticket", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Carriage", "Carriage")
                        .WithMany("Tickets")
                        .HasForeignKey("CarriageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Order", "Order")
                        .WithMany("Tickets")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Person", "Person")
                        .WithMany("Tickets")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Carriage");

                    b.Navigation("Order");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Transit", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Station", "EndStation")
                        .WithMany("TransitsWithEndStation")
                        .HasForeignKey("EndStationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Station", "StartStation")
                        .WithMany("TransitsWithStartStation")
                        .HasForeignKey("StartStationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("EndStation");

                    b.Navigation("StartStation");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Trip", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Route", "Route")
                        .WithMany("Trips")
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Train", "Train")
                        .WithMany("Trips")
                        .HasForeignKey("TrainId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Route");

                    b.Navigation("Train");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.TripStation", b =>
                {
                    b.HasOne("SpaceZD.DataLayer.Entities.Platform", "Platform")
                        .WithMany("TripStations")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SpaceZD.DataLayer.Entities.Station", "Station")
                        .WithMany("TripStations")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("SpaceZD.DataLayer.Entities.Trip", "Trip")
                        .WithMany("Stations")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Platform");

                    b.Navigation("Station");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Carriage", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.CarriageType", b =>
                {
                    b.Navigation("Carriages");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Order", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Person", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Platform", b =>
                {
                    b.Navigation("PlatformMaintenances");

                    b.Navigation("TripStations");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Route", b =>
                {
                    b.Navigation("RouteTransits");

                    b.Navigation("Trips");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Station", b =>
                {
                    b.Navigation("Platforms");

                    b.Navigation("RoutesWithEndStation");

                    b.Navigation("RoutesWithStartStation");

                    b.Navigation("TransitsWithEndStation");

                    b.Navigation("TransitsWithStartStation");

                    b.Navigation("TripStations");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Train", b =>
                {
                    b.Navigation("Carriages");

                    b.Navigation("Trips");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Transit", b =>
                {
                    b.Navigation("RouteTransit");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.Trip", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Stations");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.TripStation", b =>
                {
                    b.Navigation("OrdersWithEndStation");

                    b.Navigation("OrdersWithStartStation");
                });

            modelBuilder.Entity("SpaceZD.DataLayer.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Persons");
                });
#pragma warning restore 612, 618
        }
    }
}
