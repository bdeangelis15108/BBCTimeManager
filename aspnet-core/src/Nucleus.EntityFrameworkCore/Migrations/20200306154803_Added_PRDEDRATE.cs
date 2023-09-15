using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_PRDEDRATE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRDEDRATES",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UNIONLOCAL = table.Column<string>(maxLength: 100, nullable: true),
                    CLASS = table.Column<string>(maxLength: 100, nullable: true),
                    DEDTYPE = table.Column<int>(nullable: false),
                    PERHR = table.Column<decimal>(nullable: false),
                    UNIONNUM = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRDEDRATES", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRDEDRATES_PRCLASSs_UNIONNUM",
                        column: x => x.UNIONNUM,
                        principalTable: "PRCLASSs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRDEDRATES_TenantId",
                table: "PRDEDRATES",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PRDEDRATES_UNIONNUM",
                table: "PRDEDRATES",
                column: "UNIONNUM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRDEDRATES");
        }
    }
}
