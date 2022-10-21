using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221020173256 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "ClassDefinitions");
        }
    }
}
