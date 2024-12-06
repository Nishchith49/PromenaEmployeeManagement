using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PromenaEmployeeManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnsAT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "late_time",
                table: "Attendance",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Attendance",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "late_time",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Attendance");
        }
    }
}
