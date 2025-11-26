using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addForeignKeyCategoryToProductModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                column: "CategoryId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CategoryId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_courses_CategoryId",
                table: "courses",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_Category_CategoryId",
                table: "courses",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_Category_CategoryId",
                table: "courses");

            migrationBuilder.DropIndex(
                name: "IX_courses_CategoryId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "courses");

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 19, 9, 6, 36, 525, DateTimeKind.Utc).AddTicks(8872));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 19, 9, 6, 36, 525, DateTimeKind.Utc).AddTicks(8874));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 19, 9, 6, 36, 525, DateTimeKind.Utc).AddTicks(8875));
        }
    }
}
