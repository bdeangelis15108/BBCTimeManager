using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_StatusUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    OriginalstatusId = table.Column<int>(nullable: true),
                    TimesheetsId = table.Column<int>(nullable: true),
                    NewStatusesId = table.Column<int>(nullable: true),
                    JobsId = table.Column<int>(nullable: true),
                    ModifiedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusUpdates_Jobses_JobsId",
                        column: x => x.JobsId,
                        principalTable: "Jobses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusUpdates_AbpUsers_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusUpdates_Statuses_NewStatusesId",
                        column: x => x.NewStatusesId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusUpdates_Timesheets_TimesheetsId",
                        column: x => x.TimesheetsId,
                        principalTable: "Timesheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StatusUpdates_JobsId",
                table: "StatusUpdates",
                column: "JobsId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusUpdates_ModifiedBy",
                table: "StatusUpdates",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StatusUpdates_NewStatusesId",
                table: "StatusUpdates",
                column: "NewStatusesId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusUpdates_TimesheetsId",
                table: "StatusUpdates",
                column: "TimesheetsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusUpdates");
        }
    }
}
