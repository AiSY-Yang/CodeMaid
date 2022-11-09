using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221108105030 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Setting",
                table: "ClassDefinitions");

            migrationBuilder.AddColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Setting",
                table: "Maids");

            migrationBuilder.AddColumn<string>(
                name: "Setting",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: "序列化保存的设置")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
