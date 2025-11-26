using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace eduPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingDbsetToApplicationDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "courses",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassingScore",
                table: "courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "courses",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalMarks",
                table: "courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionA = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectOption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_courses_QuizCourseId",
                        column: x => x.QuizCourseId,
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "section",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoCourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_section_courses_VideoCourseId",
                        column: x => x.VideoCourseId,
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lecture",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LectureTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lecture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lecture_section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 8, 57, 42, 744, DateTimeKind.Utc).AddTicks(1067));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 8, 57, 42, 744, DateTimeKind.Utc).AddTicks(1070));

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 11, 24, 8, 57, 42, 744, DateTimeKind.Utc).AddTicks(1071));

            migrationBuilder.InsertData(
                table: "courses",
                columns: new[] { "Id", "CategoryId", "Description", "Discriminator", "Duration", "IsPublished", "Level", "Title" },
                values: new object[,]
                {
                    { 1, 4, "A beginner-friendly course covering the basics of C# programming, syntax, and simple projects.", "Course", "8h 30m", false, "Beginner", "Introduction to Programming with C#" },
                    { 2, 4, "Learn how to build modern web applications using ASP.NET Core MVC, Razor Pages, and Entity Framework.", "Course", "12h 15m", true, "Intermediate", "ASP.NET Core MVC for Web Development" },
                    { 3, 4, "Master advanced SQL queries, stored procedures, and database optimization techniques using Microsoft SQL Server.", "Course", "10h 45m", false, "Advanced", "Advanced Database Management with SQL Server" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_lecture_SectionId",
                table: "lecture",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuizCourseId",
                table: "Question",
                column: "QuizCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_section_VideoCourseId",
                table: "section",
                column: "VideoCourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "lecture");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "section");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "TotalMarks",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "courses");

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
    }
}
