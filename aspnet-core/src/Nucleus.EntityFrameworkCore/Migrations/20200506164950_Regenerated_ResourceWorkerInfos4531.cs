using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_ResourceWorkerInfos4531 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResourcesId",
                table: "ResourceWorkerInfoses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResourceId",
                table: "Resourceses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResourceWorkerInfoses_ResourcesId",
                table: "ResourceWorkerInfoses",
                column: "ResourcesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResourceWorkerInfoses_Resourceses_ResourcesId",
                table: "ResourceWorkerInfoses",
                column: "ResourcesId",
                principalTable: "Resourceses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResourceWorkerInfoses_Resourceses_ResourcesId",
                table: "ResourceWorkerInfoses");

            migrationBuilder.DropIndex(
                name: "IX_ResourceWorkerInfoses_ResourcesId",
                table: "ResourceWorkerInfoses");

            migrationBuilder.DropColumn(
                name: "ResourcesId",
                table: "ResourceWorkerInfoses");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "Resourceses");
        }
    }
}
