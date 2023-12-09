using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20230104111230 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: "备注",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnum",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否是枚举",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AddColumn<bool>(
                name: "AddEnumRemark",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "添加枚举的remark信息");

            migrationBuilder.AlterColumn<int>(
                name: "MaidWork",
                table: "Maids",
                type: "int",
                nullable: false,
                comment: "功能(0-配置同步功能,1-DTO同步,2-枚举remarks标签同步)",
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddEnumRemark",
                table: "Projects");

            migrationBuilder.AlterColumn<string>(
                name: "Remark",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "备注")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsEnum",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否是枚举");

            migrationBuilder.AlterColumn<int>(
                name: "MaidWork",
                table: "Maids",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "功能(0-配置同步功能,1-DTO同步,2-枚举remarks标签同步)");
        }
    }
}
