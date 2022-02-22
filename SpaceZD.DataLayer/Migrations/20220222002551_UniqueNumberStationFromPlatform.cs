using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceZD.DataLayer.Migrations
{
    public partial class UniqueNumberStationFromPlatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Platform_Number_StationId",
                table: "Platform",
                columns: new[] { "Number", "StationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Platform_Number_StationId",
                table: "Platform");
        }
    }
}
