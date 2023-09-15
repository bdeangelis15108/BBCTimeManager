using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_ResourceWorkerInfos8277 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wcomp1",
                table: "ResourceWorkerInfoses",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wcomp1",
                table: "ResourceWorkerInfoses");
        }
    }
}
