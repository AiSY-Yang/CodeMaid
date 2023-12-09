using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221122105300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Path",
                keyValue: null,
                column: "Path",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: "项目路径",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "项目路径")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LeadingTrivia",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeadingTrivia",
                table: "ClassDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Projects",
                type: "longtext",
                nullable: true,
                comment: "项目路径",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "项目路径")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
