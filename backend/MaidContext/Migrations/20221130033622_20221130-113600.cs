using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221130113600 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HasSet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否包含Set",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<bool>(
                name: "HasGet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否包含Get",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<string>(
                name: "GitBranch",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: "Git分支")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GitBranch",
                table: "Projects");

            migrationBuilder.AlterColumn<bool>(
                name: "HasSet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否包含Set");

            migrationBuilder.AlterColumn<bool>(
                name: "HasGet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否包含Get");
        }
    }
}
