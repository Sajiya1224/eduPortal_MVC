using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class VMUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 41, 57, 69, DateTimeKind.Utc).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 41, 57, 69, DateTimeKind.Utc).AddTicks(6703));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 41, 57, 69, DateTimeKind.Utc).AddTicks(6704));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 27, 54, 665, DateTimeKind.Utc).AddTicks(9791));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 27, 54, 665, DateTimeKind.Utc).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 7, 27, 54, 665, DateTimeKind.Utc).AddTicks(9795));
        }
    }
}
