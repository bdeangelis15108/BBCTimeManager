using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_Statuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    IsDefault = table.Column<bool>(nullable: false),
                    ForwardName = table.Column<string>(maxLength: 50, nullable: true),
                    ReverseName = table.Column<string>(maxLength: 50, nullable: true),
                    ForwardId = table.Column<int>(nullable: true),
                    ReverseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
