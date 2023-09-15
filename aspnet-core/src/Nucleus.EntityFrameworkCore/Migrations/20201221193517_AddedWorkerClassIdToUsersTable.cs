using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class AddedWorkerClassIdToUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkerClaseesesId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_WorkerClaseesesId",
                table: "AbpUsers",
                column: "WorkerClaseesesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_WorkerClaseeses_WorkerClaseesesId",
                table: "AbpUsers",
                column: "WorkerClaseesesId",
                principalTable: "WorkerClaseeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_WorkerClaseeses_WorkerClaseesesId",
                table: "AbpUsers");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_WorkerClaseesesId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "WorkerClaseesesId",
                table: "AbpUsers");
        }
    }
}
