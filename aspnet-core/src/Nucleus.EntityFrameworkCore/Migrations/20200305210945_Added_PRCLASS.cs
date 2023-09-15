using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_PRCLASS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRCLASSs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    NAME = table.Column<string>(maxLength: 100, nullable: true),
                    UNIONNUM = table.Column<int>(nullable: false),
                    CLASS = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRCLASSs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRCLASSs_PREMPLOYEES_CLASS",
                        column: x => x.CLASS,
                        principalTable: "PREMPLOYEES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRCLASSs_JCUNIONs_UNIONNUM",
                        column: x => x.UNIONNUM,
                        principalTable: "JCUNIONs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PRCLASSs_CLASS",
                table: "PRCLASSs",
                column: "CLASS");

            migrationBuilder.CreateIndex(
                name: "IX_PRCLASSs_TenantId",
                table: "PRCLASSs",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_PRCLASSs_UNIONNUM",
                table: "PRCLASSs",
                column: "UNIONNUM");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRCLASSs");
        }
    }
}
