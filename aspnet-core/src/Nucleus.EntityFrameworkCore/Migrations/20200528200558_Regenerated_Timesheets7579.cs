using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nucleus.Migrations
{
    public partial class Regenerated_Timesheets7579 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Timesheets",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmitedDate",
                table: "Timesheets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "SubmitedDate",
                table: "Timesheets");
        }
    }
}
