using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_JCUNION : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JCUNIONs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    UNIONNUM = table.Column<string>(maxLength: 100, nullable: false),
                    UNIONLOCAL = table.Column<string>(maxLength: 100, nullable: true),
                    JOBNUM = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JCUNIONs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JCUNIONs_JACCATs_JOBNUM",
                        column: x => x.JOBNUM,
                        principalTable: "JACCATs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JCUNIONs_JOBNUM",
                table: "JCUNIONs",
                column: "JOBNUM");

            migrationBuilder.CreateIndex(
                name: "IX_JCUNIONs_TenantId",
                table: "JCUNIONs",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JCUNIONs");
        }
    }
}
