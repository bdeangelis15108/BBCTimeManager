using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_ShiftExpenses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShiftExpenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    ShiftResourcesId = table.Column<int>(nullable: true),
                    ExpenseTypesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftExpenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftExpenses_ExpenseTypeses_ExpenseTypesId",
                        column: x => x.ExpenseTypesId,
                        principalTable: "ExpenseTypeses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftExpenses_ShiftResources_ShiftResourcesId",
                        column: x => x.ShiftResourcesId,
                        principalTable: "ShiftResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftExpenses_ExpenseTypesId",
                table: "ShiftExpenses",
                column: "ExpenseTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftExpenses_ShiftResourcesId",
                table: "ShiftExpenses",
                column: "ShiftResourcesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftExpenses");
        }
    }
}
