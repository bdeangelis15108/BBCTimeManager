using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_JACCAT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JACCATs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    SEQUENCE = table.Column<int>(nullable: false),
                    JOBNUM = table.Column<string>(maxLength: 100, nullable: false),
                    PHASENUM = table.Column<string>(maxLength: 50, nullable: true),
                    CATNUM = table.Column<string>(maxLength: 100, nullable: true),
                    NAME = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JACCATs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JACCATs_TenantId",
                table: "JACCATs",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JACCATs");
        }
    }
}
