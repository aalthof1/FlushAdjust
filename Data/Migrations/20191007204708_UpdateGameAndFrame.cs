using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class UpdateGameAndFrame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pattern",
                table: "Frame");

            migrationBuilder.AddColumn<string>(
                name: "Pattern",
                table: "Game",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "Frame",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pattern",
                table: "Game");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "Frame",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Pattern",
                table: "Frame",
                nullable: true);
        }
    }
}
