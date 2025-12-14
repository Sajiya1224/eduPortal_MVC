using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingNewFieldsInCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_AspNetUsers_ApplicationUserId",
                table: "courses");

            migrationBuilder.AddColumn<string>(
                name: "AddInfo",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "courseContent",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 4, 6, 59, 27, 925, DateTimeKind.Utc).AddTicks(6927));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 4, 6, 59, 27, 925, DateTimeKind.Utc).AddTicks(6929));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 4, 6, 59, 27, 925, DateTimeKind.Utc).AddTicks(6930));

            migrationBuilder.AddForeignKey(
                name: "FK_courses_AspNetUsers_ApplicationUserId",
                table: "courses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_AspNetUsers_ApplicationUserId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "AddInfo",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "courseContent",
                table: "courses");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 10, 12, 49, 805, DateTimeKind.Utc).AddTicks(9530));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 10, 12, 49, 805, DateTimeKind.Utc).AddTicks(9536));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 10, 12, 49, 805, DateTimeKind.Utc).AddTicks(9537));

            migrationBuilder.AddForeignKey(
                name: "FK_courses_AspNetUsers_ApplicationUserId",
                table: "courses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
