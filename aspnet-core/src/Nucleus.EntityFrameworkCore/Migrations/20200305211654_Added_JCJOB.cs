using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_JCJOB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JCJOBs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    STATE = table.Column<string>(maxLength: 50, nullable: true),
                    LOCALITY = table.Column<string>(maxLength: 100, nullable: true),
                    CLASS = table.Column<string>(maxLength: 10, nullable: true),
                    CLOSED = table.Column<int>(nullable: false),
                    JOBNUM = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JCJOBs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JCJOBs_JACCATs_JOBNUM",
                        column: x => x.JOBNUM,
                        principalTable: "JACCATs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JCJOBs_JOBNUM",
                table: "JCJOBs",
                column: "JOBNUM");

            migrationBuilder.CreateIndex(
                name: "IX_JCJOBs_TenantId",
                table: "JCJOBs",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JCJOBs");
        }
    }
}
