using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_Timetables2835 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_ExpenseTypeses_CostTypeId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_CostTypeId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "CostTypeId",
                table: "Timetables");

            migrationBuilder.AddColumn<int>(
                name: "CostTypesId",
                table: "Timetables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostTypesId",
                table: "Timetables",
                column: "CostTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_CostTypese_CostTypesId",
                table: "Timetables",
                column: "CostTypesId",
                principalTable: "CostTypese",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_CostTypese_CostTypesId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_CostTypesId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "CostTypesId",
                table: "Timetables");

            migrationBuilder.AddColumn<int>(
                name: "CostTypeId",
                table: "Timetables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostTypeId",
                table: "Timetables",
                column: "CostTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_ExpenseTypeses_CostTypeId",
                table: "Timetables",
                column: "CostTypeId",
                principalTable: "ExpenseTypeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
