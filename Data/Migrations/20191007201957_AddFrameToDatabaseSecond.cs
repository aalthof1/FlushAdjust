using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BowlingApp.Data.Migrations
{
    public partial class AddFrameToDatabaseSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frame",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PinsLeft = table.Column<string>(nullable: true),
                    PinsLeftTenth = table.Column<string>(nullable: true),
                    Result = table.Column<string>(nullable: true),
                    ExtraFirst = table.Column<int>(nullable: false),
                    ExtraSecond = table.Column<int>(nullable: false),
                    Split = table.Column<bool>(nullable: false),
                    BallId = table.Column<int>(nullable: false),
                    Pattern = table.Column<string>(nullable: true),
                    FootPos = table.Column<string>(nullable: true),
                    BallSpeed = table.Column<double>(nullable: false),
                    Arrows = table.Column<int>(nullable: false),
                    Breakpoint = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frame", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Frame");
        }
    }
}
