using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_Timetables2581 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_ShiftResources_CostCode",
                table: "Timetables");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_CostTypese_CostTypesId",
                table: "Timetables");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_ShiftResources_PaytypeId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_CostCode",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_CostTypesId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_PaytypeId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "CostTypesId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "PaytypeId",
                table: "Timetables");

            migrationBuilder.AlterColumn<string>(
                name: "CostCode",
                table: "Timetables",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostTypeId",
                table: "Timetables",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayTypesId",
                table: "Timetables",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostTypeId",
                table: "Timetables",
                column: "CostTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_PayTypesId",
                table: "Timetables",
                column: "PayTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_ExpenseTypeses_CostTypeId",
                table: "Timetables",
                column: "CostTypeId",
                principalTable: "ExpenseTypeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_PayTypeses_PayTypesId",
                table: "Timetables",
                column: "PayTypesId",
                principalTable: "PayTypeses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_ExpenseTypeses_CostTypeId",
                table: "Timetables");

            migrationBuilder.DropForeignKey(
                name: "FK_Timetables_PayTypeses_PayTypesId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_CostTypeId",
                table: "Timetables");

            migrationBuilder.DropIndex(
                name: "IX_Timetables_PayTypesId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "CostTypeId",
                table: "Timetables");

            migrationBuilder.DropColumn(
                name: "PayTypesId",
                table: "Timetables");

            migrationBuilder.AlterColumn<int>(
                name: "CostCode",
                table: "Timetables",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "CostTypesId",
                table: "Timetables",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaytypeId",
                table: "Timetables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostCode",
                table: "Timetables",
                column: "CostCode");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_CostTypesId",
                table: "Timetables",
                column: "CostTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_PaytypeId",
                table: "Timetables",
                column: "PaytypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_ShiftResources_CostCode",
                table: "Timetables",
                column: "CostCode",
                principalTable: "ShiftResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_CostTypese_CostTypesId",
                table: "Timetables",
                column: "CostTypesId",
                principalTable: "CostTypese",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timetables_ShiftResources_PaytypeId",
                table: "Timetables",
                column: "PaytypeId",
                principalTable: "ShiftResources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
