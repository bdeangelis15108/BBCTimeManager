using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_JobCategories1154 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EQUIPMENTS_TenantId",
                table: "EQUIPMENTS");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "EQUIPMENTS");

            migrationBuilder.AddColumn<int>(
                name: "JobPhaseCodesId",
                table: "JobCategories",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EQUIPNUM",
                table: "EQUIPMENTS",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "EQUIPNUM",
                table: "EQUIPMENTS",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 55,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "EQUIPMENTS",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EQUIPMENTS_TenantId",
                table: "EQUIPMENTS",
                column: "TenantId");
        }
    }
}
