using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Added_Timetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Timetables",
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
                    CostCode = table.Column<int>(nullable: true),
                    Rate = table.Column<int>(nullable: true),
                    Unionlocal = table.Column<int>(nullable: true),
                    State = table.Column<int>(nullable: true),
                    Description = table.Column<int>(nullable: true),
                    CostTypesId = table.Column<int>(nullable: true),
                    AccountsId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    PaytypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timetables_Accounts_AccountsId",
                        column: x => x.AccountsId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_ShiftResources_CostCode",
                        column: x => x.CostCode,
                        principalTable: "ShiftResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_CostTypese_CostTypesId",
                        column: x => x.CostTypesId,
                        principalTable: "CostTypese",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_AbpUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_ExpenseTypeses_Description",
                        column: x => x.Description,
                        principalTable: "ExpenseTypeses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_ShiftResources_PaytypeId",
                        column: x => x.PaytypeId,
                        principalTable: "ShiftResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_PayPeriods_PeriodDate",
                        column: x => x.PeriodDate,
                        principalTable: "PayPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_UnionPayRates_Rate",
                        column: x => x.Rate,
                        principalTable: "UnionPayRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_Resourceses_ResourcesCode",
                        column: x => x.ResourcesCode,
                        principalTable: "Resourceses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_Addresseses_State",
                        column: x => x.State,
                        principalTable: "Addresseses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetables_Unions_Unionlocal",
                        column: x => x.Unionlocal,
                        principalTable: "Unions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_AccountsId",
                table: "Timetables",
                column: "AccountsId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostCode",
                table: "Timetables",
                column: "CostCode");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostTypesId",
                table: "Timetables",
                column: "CostTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CreatedBy",
                table: "Timetables",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_Description",
                table: "Timetables",
                column: "Description");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_PaytypeId",
                table: "Timetables",
                column: "PaytypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_PeriodDate",
                table: "Timetables",
                column: "PeriodDate");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_Rate",
                table: "Timetables",
                column: "Rate");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_ResourcesCode",
                table: "Timetables",
                column: "ResourcesCode");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_State",
                table: "Timetables",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_Unionlocal",
                table: "Timetables",
                column: "Unionlocal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetables");
        }
    }
}
