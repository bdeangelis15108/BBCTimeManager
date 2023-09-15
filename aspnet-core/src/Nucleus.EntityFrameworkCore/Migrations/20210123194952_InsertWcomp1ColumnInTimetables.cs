using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class InsertWcomp1ColumnInTimetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Wcomp1",
                table: "Timetables",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wcomp1",
                table: "ResourceWorkerInfoses",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wcomp1",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "Wcomp1",
                table: "ResourceWorkerInfoses");
        }
    }
}
