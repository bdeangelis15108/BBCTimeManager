using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_ResourceWorkerInfos7653 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "ResourceWorkerInfoses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceWorkerInfoses_UserId",
                table: "ResourceWorkerInfoses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceWorkerInfoses_AbpUsers_UserId",
                table: "ResourceWorkerInfoses",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceWorkerInfoses_AbpUsers_UserId",
                table: "ResourceWorkerInfoses");

            migrationBuilder.DropIndex(
                name: "IX_ResourceWorkerInfoses_UserId",
                table: "ResourceWorkerInfoses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ResourceWorkerInfoses");
        }
    }
}
