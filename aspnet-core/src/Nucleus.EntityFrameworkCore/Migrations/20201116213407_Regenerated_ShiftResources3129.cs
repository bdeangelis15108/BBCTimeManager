using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_ShiftResources3129 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerClaseesId",
                table: "ShiftResources",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_WorkerClaseesId",
                table: "ShiftResources",
                column: "WorkerClaseesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftResources_WorkerClaseeses_WorkerClaseesId",
                table: "ShiftResources",
                column: "WorkerClaseesId",
                principalTable: "WorkerClaseeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShiftResources_WorkerClaseeses_WorkerClaseesId",
                table: "ShiftResources");

            migrationBuilder.DropIndex(
                name: "IX_ShiftResources_WorkerClaseesId",
                table: "ShiftResources");

            migrationBuilder.DropColumn(
                name: "WorkerClaseesId",
                table: "ShiftResources");
        }
    }
}
