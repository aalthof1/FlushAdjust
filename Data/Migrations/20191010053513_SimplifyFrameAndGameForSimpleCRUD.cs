using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class SimplifyFrameAndGameForSimpleCRUD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pattern",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Game");

            migrationBuilder.DropColumn(
                name: "Arrows",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "BallId",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "BallSpeed",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "Breakpoint",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "ExtraFirst",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "ExtraSecond",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "FootPos",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "PinsLeftTenth",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "Split",
                table: "Frame");

            migrationBuilder.RenameColumn(
                name: "Result",
                table: "Frame",
                newName: "frameString");

            migrationBuilder.AlterColumn<string>(
                name: "PinsLeft",
                table: "Frame",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "frameString",
                table: "Frame",
                newName: "Result");

            migrationBuilder.AddColumn<string>(
                name: "Pattern",
                table: "Game",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Game",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PinsLeft",
                table: "Frame",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Arrows",
                table: "Frame",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BallId",
                table: "Frame",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BallSpeed",
                table: "Frame",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Breakpoint",
                table: "Frame",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExtraFirst",
                table: "Frame",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExtraSecond",
                table: "Frame",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FootPos",
                table: "Frame",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PinsLeftTenth",
                table: "Frame",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Split",
                table: "Frame",
                nullable: false,
                defaultValue: false);
        }
    }
}
