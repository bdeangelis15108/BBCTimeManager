using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_Jobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jobses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    AddressesId = table.Column<int>(nullable: true),
                    JobClassesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobses_Addresseses_AddressesId",
                        column: x => x.AddressesId,
                        principalTable: "Addresseses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jobses_JobClasseses_JobClassesId",
                        column: x => x.JobClassesId,
                        principalTable: "JobClasseses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobses_AddressesId",
                table: "Jobses",
                column: "AddressesId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobses_JobClassesId",
                table: "Jobses",
                column: "JobClassesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobses");
        }
    }
}
