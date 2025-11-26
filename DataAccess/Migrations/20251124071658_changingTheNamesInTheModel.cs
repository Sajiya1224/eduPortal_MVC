using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changingTheNamesInTheModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 10, 44, 56, 929, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 10, 44, 56, 929, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 10, 44, 56, 929, DateTimeKind.Utc).AddTicks(7150));
        }
    }
}
