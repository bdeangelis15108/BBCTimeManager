using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_JobUnions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobUnions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(maxLength: 50, nullable: true),
                    JobsId = table.Column<int>(nullable: true),
                    UnionsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobUnions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobUnions_Jobses_JobsId",
                        column: x => x.JobsId,
                        principalTable: "Jobses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobUnions_Unions_UnionsId",
                        column: x => x.UnionsId,
                        principalTable: "Unions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobUnions_JobsId",
                table: "JobUnions",
                column: "JobsId");

            migrationBuilder.CreateIndex(
                name: "IX_JobUnions_UnionsId",
                table: "JobUnions",
                column: "UnionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobUnions");
        }
    }
}
