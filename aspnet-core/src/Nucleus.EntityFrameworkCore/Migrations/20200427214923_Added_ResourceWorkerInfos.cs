using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_ResourceWorkerInfos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceWorkerInfoses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    WorkerClaseesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceWorkerInfoses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceWorkerInfoses_WorkerClaseeses_WorkerClaseesId",
                        column: x => x.WorkerClaseesId,
                        principalTable: "WorkerClaseeses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceWorkerInfoses_WorkerClaseesId",
                table: "ResourceWorkerInfoses",
                column: "WorkerClaseesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceWorkerInfoses");
        }
    }
}
