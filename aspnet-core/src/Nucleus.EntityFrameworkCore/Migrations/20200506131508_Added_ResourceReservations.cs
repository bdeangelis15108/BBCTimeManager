using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_ResourceReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceReservationses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservedFrom = table.Column<DateTime>(nullable: true),
                    ReservedUntil = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ResourcesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceReservationses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceReservationses_Resourceses_ResourcesId",
                        column: x => x.ResourcesId,
                        principalTable: "Resourceses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResourceReservationses_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceReservationses_ResourcesId",
                table: "ResourceReservationses",
                column: "ResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceReservationses_UserId",
                table: "ResourceReservationses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceReservationses");
        }
    }
}
