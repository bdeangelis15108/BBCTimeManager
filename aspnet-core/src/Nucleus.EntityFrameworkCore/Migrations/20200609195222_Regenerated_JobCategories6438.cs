using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_JobCategories6438 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobCategories_JobPhaseCodes_JobPhaseCodesId",
                table: "JobCategories");

            migrationBuilder.DropIndex(
                name: "IX_JobCategories_JobPhaseCodesId",
                table: "JobCategories");

            migrationBuilder.DropColumn(
                name: "JobPhaseCodesId",
                table: "JobCategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobPhaseCodesId",
                table: "JobCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobCategories_JobPhaseCodesId",
                table: "JobCategories",
                column: "JobPhaseCodesId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobCategories_JobPhaseCodes_JobPhaseCodesId",
                table: "JobCategories",
                column: "JobPhaseCodesId",
                principalTable: "JobPhaseCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
