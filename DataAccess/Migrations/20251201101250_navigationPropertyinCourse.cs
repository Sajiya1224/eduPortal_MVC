using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class navigationPropertyinCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorId",
                table: "courses");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "courses",
                type: "nvarchar(450)",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_courses_ApplicationUserId",
                table: "courses",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_AspNetUsers_ApplicationUserId",
                table: "courses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_AspNetUsers_ApplicationUserId",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_ApplicationUserId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "courses");

            migrationBuilder.AddColumn<string>(
                name: "InstructorId",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 9, 10, 1, 756, DateTimeKind.Utc).AddTicks(3996));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 9, 10, 1, 756, DateTimeKind.Utc).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 9, 10, 1, 756, DateTimeKind.Utc).AddTicks(4000));
        }
    }
}
