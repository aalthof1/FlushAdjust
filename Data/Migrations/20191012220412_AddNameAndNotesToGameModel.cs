using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class AddNameAndNotesToGameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Game",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Game",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Game");
        }
    }
}
