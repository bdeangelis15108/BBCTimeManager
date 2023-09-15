using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_Timetables3263 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerClaseesId",
                table: "Timetables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_WorkerClaseesId",
                table: "Timetables",
                column: "WorkerClaseesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_WorkerClaseeses_WorkerClaseesId",
                table: "Timetables",
                column: "WorkerClaseesId",
                principalTable: "WorkerClaseeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_WorkerClaseeses_WorkerClaseesId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_WorkerClaseesId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "WorkerClaseesId",
                table: "Timetables");
        }
    }
}
