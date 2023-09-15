using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_ECCOST1803 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ECCOSTS_TenantId",
                table: "ECCOSTS");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ECCOSTS");

            migrationBuilder.AlterColumn<string>(
                name: "ESTHOURLY",
                table: "ECCOSTS",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ESTHOURLY",
                table: "ECCOSTS",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ECCOSTS",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ECCOSTS_TenantId",
                table: "ECCOSTS",
                column: "TenantId");
        }
    }
}
