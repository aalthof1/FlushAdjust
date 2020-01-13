using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class attemptToAddFramesToGameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Frame",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Frame_GameId",
                table: "Frame",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Frame_Game_GameId",
                table: "Frame",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Frame_Game_GameId",
                table: "Frame");

            migrationBuilder.DropIndex(
                name: "IX_Frame_GameId",
                table: "Frame");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Frame");
        }
    }
}
