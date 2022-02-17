using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpaceZD.DataLayer.Migrations
{
    public partial class AddPersonsFromUserAndNullableProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PlatformId",
                table: "TripStation",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Person",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Person_UserId",
                table: "Person",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_User_UserId",
                table: "Person",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_User_UserId",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_UserId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Person");

            migrationBuilder.AlterColumn<int>(
                name: "PlatformId",
                table: "TripStation",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
