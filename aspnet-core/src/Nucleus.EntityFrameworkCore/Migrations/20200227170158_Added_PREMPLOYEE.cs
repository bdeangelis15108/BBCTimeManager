using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_PREMPLOYEE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PREMPLOYEES",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    EMPNUM = table.Column<int>(nullable: false),
                    NAME = table.Column<string>(maxLength: 100, nullable: false),
                    UNIONNUM = table.Column<string>(maxLength: 100, nullable: true),
                    UNIONLOCAL = table.Column<string>(maxLength: 100, nullable: true),
                    CLASS = table.Column<string>(maxLength: 100, nullable: false),
                    WCOMPNUM1 = table.Column<string>(maxLength: 100, nullable: true),
                    LASTNAME = table.Column<string>(maxLength: 100, nullable: false),
                    FIRSTNAME = table.Column<string>(maxLength: 100, nullable: false),
                    STATUS = table.Column<string>(maxLength: 100, nullable: false),
                    PAYRATE = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PREMPLOYEES", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PREMPLOYEES_TenantId",
                table: "PREMPLOYEES",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PREMPLOYEES");
        }
    }
}
