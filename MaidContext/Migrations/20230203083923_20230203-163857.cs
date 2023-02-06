using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20230203163857 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EnumDefinitionId",
                table: "PropertyDefinitions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AddEnumRemark",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否添加枚举的remark信息",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "添加枚举的remark信息");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDefinitions_EnumDefinitionId",
                table: "PropertyDefinitions",
                column: "EnumDefinitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDefinitions_EnumDefinitions_EnumDefinitionId",
                table: "PropertyDefinitions",
                column: "EnumDefinitionId",
                principalTable: "EnumDefinitions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDefinitions_EnumDefinitions_EnumDefinitionId",
                table: "PropertyDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDefinitions_EnumDefinitionId",
                table: "PropertyDefinitions");

            migrationBuilder.DropColumn(
                name: "EnumDefinitionId",
                table: "PropertyDefinitions");

            migrationBuilder.AlterColumn<bool>(
                name: "AddEnumRemark",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                comment: "添加枚举的remark信息",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否添加枚举的remark信息");
        }
    }
}
