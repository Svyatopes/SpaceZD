using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceZD.DataLayer.Migrations
{
    public partial class UniqueStartEndStationFromTransit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transit_StartStationId",
                table: "Transit");

            migrationBuilder.CreateIndex(
                name: "IX_Transit_StartStationId_EndStationId",
                table: "Transit",
                columns: new[] { "StartStationId", "EndStationId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transit_StartStationId_EndStationId",
                table: "Transit");

            migrationBuilder.CreateIndex(
                name: "IX_Transit_StartStationId",
                table: "Transit",
                column: "StartStationId");
        }
    }
}
