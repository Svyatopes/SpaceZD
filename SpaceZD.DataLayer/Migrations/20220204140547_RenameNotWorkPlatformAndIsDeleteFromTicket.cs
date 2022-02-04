using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceZD.DataLayer.Migrations
{
    public partial class RenameNotWorkPlatformAndIsDeleteFromTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotWorkPlatform");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "Ticket",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Trip",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Ticket",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "PlatformMaintenance",
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
                    table.PrimaryKey("PK_PlatformMaintenance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlatformMaintenance_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlatformMaintenance_PlatformId",
                table: "PlatformMaintenance",
                column: "PlatformId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlatformMaintenance");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Trip");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Ticket",
                newName: "isDeleted");

            migrationBuilder.AlterColumn<bool>(
                name: "isDeleted",
                table: "Ticket",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.CreateTable(
                name: "NotWorkPlatform",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformId = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_NotWorkPlatform_PlatformId",
                table: "NotWorkPlatform",
                column: "PlatformId");
        }
    }
}
