using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_EquipTimetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipTimetables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day1 = table.Column<decimal>(nullable: true),
                    Day2 = table.Column<decimal>(nullable: true),
                    Day3 = table.Column<decimal>(nullable: true),
                    Day4 = table.Column<decimal>(nullable: true),
                    Day5 = table.Column<decimal>(nullable: true),
                    Day6 = table.Column<decimal>(nullable: true),
                    Day7 = table.Column<decimal>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    PeriodDate = table.Column<int>(nullable: true),
                    ResourcesCode = table.Column<int>(nullable: true),
                    PhaseCode = table.Column<int>(nullable: true),
                    CategoryCode = table.Column<int>(nullable: true),
                    JobCode = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipTimetables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipTimetables_JobCategories_CategoryCode",
                        column: x => x.CategoryCode,
                        principalTable: "JobCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipTimetables_Jobses_JobCode",
                        column: x => x.JobCode,
                        principalTable: "Jobses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipTimetables_PayPeriods_PeriodDate",
                        column: x => x.PeriodDate,
                        principalTable: "PayPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipTimetables_JobPhaseCodes_PhaseCode",
                        column: x => x.PhaseCode,
                        principalTable: "JobPhaseCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipTimetables_Resourceses_ResourcesCode",
                        column: x => x.ResourcesCode,
                        principalTable: "Resourceses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquipTimetables_CategoryCode",
                table: "EquipTimetables",
                column: "CategoryCode");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTimetables_JobCode",
                table: "EquipTimetables",
                column: "JobCode");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTimetables_PeriodDate",
                table: "EquipTimetables",
                column: "PeriodDate");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTimetables_PhaseCode",
                table: "EquipTimetables",
                column: "PhaseCode");

            migrationBuilder.CreateIndex(
                name: "IX_EquipTimetables_ResourcesCode",
                table: "EquipTimetables",
                column: "ResourcesCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquipTimetables");
        }
    }
}
