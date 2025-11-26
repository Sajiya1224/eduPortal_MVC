using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removedPricePropertyAndAddedTwoNewModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "courses");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 9, 38, 17, 718, DateTimeKind.Utc).AddTicks(4740));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 9, 38, 17, 718, DateTimeKind.Utc).AddTicks(4742));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 9, 38, 17, 718, DateTimeKind.Utc).AddTicks(4743));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "courses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 7, 21, 59, 429, DateTimeKind.Utc).AddTicks(1939));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 7, 21, 59, 429, DateTimeKind.Utc).AddTicks(1943));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 21, 7, 21, 59, 429, DateTimeKind.Utc).AddTicks(1944));

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 199.0);

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 299.0);

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Price",
                value: 249.0);
        }
    }
}
