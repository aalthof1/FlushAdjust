using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class AddGameStringToGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameString",
                table: "Game",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameString",
                table: "Game");
        }
    }
}
