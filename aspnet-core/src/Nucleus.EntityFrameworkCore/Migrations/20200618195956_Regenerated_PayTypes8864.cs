using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_PayTypes8864 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Section1",
                table: "PayTypeses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Section2",
                table: "PayTypeses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Section3",
                table: "PayTypeses",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Section1",
                table: "PayTypeses");

            migrationBuilder.DropColumn(
                name: "Section2",
                table: "PayTypeses");

            migrationBuilder.DropColumn(
                name: "Section3",
                table: "PayTypeses");
        }
    }
}
