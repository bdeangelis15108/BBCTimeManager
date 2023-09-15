using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_EmployeeUnions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeUnions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocalNumber = table.Column<string>(maxLength: 50, nullable: true),
                    UnionsId = table.Column<int>(nullable: true),
                    ResourcesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeUnions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeUnions_Resourceses_ResourcesId",
                        column: x => x.ResourcesId,
                        principalTable: "Resourceses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeUnions_Unions_UnionsId",
                        column: x => x.UnionsId,
                        principalTable: "Unions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUnions_ResourcesId",
                table: "EmployeeUnions",
                column: "ResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeUnions_UnionsId",
                table: "EmployeeUnions",
                column: "UnionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeUnions");
        }
    }
}
