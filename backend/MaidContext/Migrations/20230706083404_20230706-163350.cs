using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    /// <inheritdoc />
    public partial class _20230706163350 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "AttributeDefinitions",
                comment: "属性定义",
                oldComment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<long>(
                name: "ClassDefinitionId",
                table: "PropertyDefinitions",
                type: "bigint",
                nullable: false,
                comment: "所属类Id",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.UpdateData(
                table: "Maids",
                keyColumn: "Setting",
                keyValue: null,
                column: "Setting",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: "设置",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "序列化保存的设置")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "MaidWork",
                table: "Maids",
                type: "int",
                nullable: false,
                comment: "功能(0-配置同步功能,1-DTO同步,2-HTTP客户端生成)",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "功能(0-配置同步功能,1-DTO同步,0-sasas,0-,1-test3,40-test4,41-test5,2-枚举remarks标签同步)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumMemberDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Modifiers",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: "修饰符",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "MaidId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: false,
                comment: "Maid对象Id",
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ClassDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<int>(
                name: "MemberType",
                table: "ClassDefinitions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "更新时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AttributeDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否有效",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "创建时间",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberType",
                table: "ClassDefinitions");

            migrationBuilder.AlterTable(
                name: "AttributeDefinitions",
                comment: "类定义",
                oldComment: "属性定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<long>(
                name: "ClassDefinitionId",
                table: "PropertyDefinitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "所属类Id");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: true,
                comment: "序列化保存的设置",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "设置")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "MaidWork",
                table: "Maids",
                type: "int",
                nullable: false,
                comment: "功能(0-配置同步功能,1-DTO同步,0-sasas,0-,1-test3,40-test4,41-test5,2-枚举remarks标签同步)",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "功能(0-配置同步功能,1-DTO同步,2-HTTP客户端生成)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumMemberDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<string>(
                name: "Modifiers",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "修饰符")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "MaidId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Maid对象Id");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ClassDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "更新时间");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AttributeDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否有效");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "创建时间");
        }
    }
}
