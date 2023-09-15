using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_ECCOST : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ECCOSTS",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    CODENUM = table.Column<string>(maxLength: 10, nullable: true),
                    ESTHOURLY = table.Column<string>(maxLength: 10, nullable: true),
                    EQUIPNUM = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECCOSTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECCOSTS_EQUIPMENTS_EQUIPNUM",
                        column: x => x.EQUIPNUM,
                        principalTable: "EQUIPMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ECCOSTS_EQUIPNUM",
                table: "ECCOSTS",
                column: "EQUIPNUM");

            migrationBuilder.CreateIndex(
                name: "IX_ECCOSTS_TenantId",
                table: "ECCOSTS",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECCOSTS");
        }
    }
}
