using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removingPaymentFromTheCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Cart_cartId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_cartId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "cartId",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntendId",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 6, 55, 1, 25, DateTimeKind.Utc).AddTicks(6695));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 6, 55, 1, 25, DateTimeKind.Utc).AddTicks(6699));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 16, 6, 55, 1, 25, DateTimeKind.Utc).AddTicks(6700));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentIntendId",
                table: "Payment");

            migrationBuilder.AddColumn<int>(
                name: "cartId",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 15, 9, 39, 34, 183, DateTimeKind.Utc).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 15, 9, 39, 34, 183, DateTimeKind.Utc).AddTicks(2876));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 12, 15, 9, 39, 34, 183, DateTimeKind.Utc).AddTicks(2878));

            migrationBuilder.CreateIndex(
                name: "IX_Payment_cartId",
                table: "Payment",
                column: "cartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Cart_cartId",
                table: "Payment",
                column: "cartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
