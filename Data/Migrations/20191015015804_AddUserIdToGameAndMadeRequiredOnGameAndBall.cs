using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class AddUserIdToGameAndMadeRequiredOnGameAndBall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Game",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ball",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Game");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Ball",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
