using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_UnionPayRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnionPayRates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Class = table.Column<string>(maxLength: 10, nullable: true),
                    Dedtype = table.Column<string>(maxLength: 10, nullable: true),
                    Perhour = table.Column<decimal>(nullable: false),
                    UnionsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnionPayRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnionPayRates_Unions_UnionsId",
                        column: x => x.UnionsId,
                        principalTable: "Unions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnionPayRates_UnionsId",
                table: "UnionPayRates",
                column: "UnionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnionPayRates");
        }
    }
}
