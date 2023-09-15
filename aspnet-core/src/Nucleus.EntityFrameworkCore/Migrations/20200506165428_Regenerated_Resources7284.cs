using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_Resources7284 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Resourceses");

            migrationBuilder.AddColumn<string>(
                name: "ResourceNumber",
                table: "Resourceses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceNumber",
                table: "Resourceses");

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "Resourceses",
                type: "int",
                nullable: true);
        }
    }
}
