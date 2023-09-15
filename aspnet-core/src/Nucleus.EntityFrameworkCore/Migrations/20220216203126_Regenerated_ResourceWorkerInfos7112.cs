using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_ResourceWorkerInfos7112 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wcomp1",
                table: "ResourceWorkerInfoses");

            migrationBuilder.AddColumn<string>(
                name: "UnionLocal",
                table: "ResourceWorkerInfoses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnionNumber",
                table: "ResourceWorkerInfoses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnionLocal",
                table: "ResourceWorkerInfoses");

            migrationBuilder.DropColumn(
                name: "UnionNumber",
                table: "ResourceWorkerInfoses");

            migrationBuilder.AddColumn<string>(
                name: "Wcomp1",
                table: "ResourceWorkerInfoses",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }
    }
}
