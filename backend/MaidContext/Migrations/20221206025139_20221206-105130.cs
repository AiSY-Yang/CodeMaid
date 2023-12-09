using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221206105130 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "EnumMemberDefinitions",
                comment: " 枚举成员定义  ",
                oldComment: "枚举成员定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "EnumMemberDefinitions",
                type: "int",
                nullable: false,
                comment: " 枚举值  ",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "枚举值");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 注释  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "注释")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 枚举名称  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "枚举名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 描述  ")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "EnumMemberDefinitions");

            migrationBuilder.AlterTable(
                name: "EnumMemberDefinitions",
                comment: "枚举成员定义",
                oldComment: " 枚举成员定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "EnumMemberDefinitions",
                type: "int",
                nullable: false,
                comment: "枚举值",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: " 枚举值  ");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: true,
                comment: "注释",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 注释  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: false,
                comment: "枚举名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 枚举名称  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
