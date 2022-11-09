using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221108090956 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Using",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: "类引用的命名空间")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Using",
                table: "ClassDefinitions");
        }
    }
}
