using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingLanguagePropertyInVideoCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 19, 32, 232, DateTimeKind.Utc).AddTicks(8252));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 19, 32, 232, DateTimeKind.Utc).AddTicks(8254));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 19, 32, 232, DateTimeKind.Utc).AddTicks(8254));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 16, 57, 250, DateTimeKind.Utc).AddTicks(3396));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 16, 57, 250, DateTimeKind.Utc).AddTicks(3399));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 16, 57, 250, DateTimeKind.Utc).AddTicks(3400));
        }
    }
}
