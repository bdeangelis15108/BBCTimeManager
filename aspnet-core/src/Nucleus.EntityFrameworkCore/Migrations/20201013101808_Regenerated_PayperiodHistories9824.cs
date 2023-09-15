using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_PayperiodHistories9824 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayperiodHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    period = table.Column<string>(maxLength: 20, nullable: true),
                    active = table.Column<bool>(nullable: false),
                    PayPeriodsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayperiodHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayperiodHistories_PayPeriods_PayPeriodsId",
                        column: x => x.PayPeriodsId,
                        principalTable: "PayPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayperiodHistories_PayPeriodsId",
                table: "PayperiodHistories",
                column: "PayPeriodsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayperiodHistories");
        }
    }
}
