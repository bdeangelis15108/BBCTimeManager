using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_ShiftResources : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShiftResources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoursWorked = table.Column<decimal>(nullable: true),
                    ResourcesId = table.Column<int>(nullable: false),
                    PayTypesId = table.Column<int>(nullable: true),
                    JobPhaseCodesId = table.Column<int>(nullable: true),
                    JobCategoriesId = table.Column<int>(nullable: true),
                    TimesheetsId = table.Column<int>(nullable: true),
                    ShiftsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShiftResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShiftResources_JobCategories_JobCategoriesId",
                        column: x => x.JobCategoriesId,
                        principalTable: "JobCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftResources_JobPhaseCodes_JobPhaseCodesId",
                        column: x => x.JobPhaseCodesId,
                        principalTable: "JobPhaseCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftResources_PayTypeses_PayTypesId",
                        column: x => x.PayTypesId,
                        principalTable: "PayTypeses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftResources_Resourceses_ResourcesId",
                        column: x => x.ResourcesId,
                        principalTable: "Resourceses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShiftResources_Shifts_ShiftsId",
                        column: x => x.ShiftsId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShiftResources_Timesheets_TimesheetsId",
                        column: x => x.TimesheetsId,
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_JobCategoriesId",
                table: "ShiftResources",
                column: "JobCategoriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_JobPhaseCodesId",
                table: "ShiftResources",
                column: "JobPhaseCodesId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_PayTypesId",
                table: "ShiftResources",
                column: "PayTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_ResourcesId",
                table: "ShiftResources",
                column: "ResourcesId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_ShiftsId",
                table: "ShiftResources",
                column: "ShiftsId");

            migrationBuilder.CreateIndex(
                name: "IX_ShiftResources_TimesheetsId",
                table: "ShiftResources",
                column: "TimesheetsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShiftResources");
        }
    }
}
