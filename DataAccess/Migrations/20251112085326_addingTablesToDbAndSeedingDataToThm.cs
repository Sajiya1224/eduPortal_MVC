using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingTablesToDbAndSeedingDataToThm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 12, 8, 53, 26, 280, DateTimeKind.Utc).AddTicks(8590), "Database" },
                    { 2, new DateTime(2025, 11, 12, 8, 53, 26, 280, DateTimeKind.Utc).AddTicks(8592), "ASP.net" },
                    { 3, new DateTime(2025, 11, 12, 8, 53, 26, 280, DateTimeKind.Utc).AddTicks(8593), "Networking" }
                });

            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "Id", "Description", "Duration", "IsPublished", "Level", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "A beginner-friendly course covering the basics of C# programming, syntax, and simple projects.", "8h 30m", false, "Beginner", 199.0, "Introduction to Programming with C#" },
                    { 2, "Learn how to build modern web applications using ASP.NET Core MVC, Razor Pages, and Entity Framework.", "12h 15m", true, "Intermediate", 299.0, "ASP.NET Core MVC for Web Development" },
                    { 3, "Master advanced SQL queries, stored procedures, and database optimization techniques using Microsoft SQL Server.", "10h 45m", false, "Advanced", 249.0, "Advanced Database Management with SQL Server" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
