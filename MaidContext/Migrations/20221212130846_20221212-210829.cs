using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221212210829 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "PropertyDefinitions",
                comment: "类定义",
                oldComment: " 类定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "Projects",
                comment: "项目定义",
                oldComment: " 项目定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "Maids",
                comment: "功能",
                oldComment: " 功能  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "EnumMemberDefinitions",
                comment: "枚举成员定义",
                oldComment: " 枚举成员定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "EnumDefinitions",
                comment: "枚举定义",
                oldComment: " 枚举定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "ClassDefinitions",
                comment: "类定义",
                oldComment: " 类定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "AttributeDefinitions",
                comment: "类定义",
                oldComment: " 类定义  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "数据类型",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 数据类型  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "PropertyDefinitions",
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
                name: "Set",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: "Set方法体",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " Set方法体  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "属性名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 属性名称  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Modifiers",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "修饰符",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 修饰符  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: "前导",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 前导  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Initializer",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: "初始化器",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 初始化器  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "HasSet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否包含Set",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: " 是否包含Set  ");

            migrationBuilder.AlterColumn<bool>(
                name: "HasGet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否包含Get",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: " 是否包含Get  ");

            migrationBuilder.AlterColumn<string>(
                name: "Get",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: "Get方法体",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " Get方法体  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FullText",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "完整文本内容",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 完整文本内容  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnum",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: "项目路径",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 项目路径  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: "项目名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 项目名  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "GitBranch",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: "Git分支",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " Git分支  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "SourcePath",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: "原路径",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 原路径  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: true,
                comment: "序列化保存的设置",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 序列化保存的设置  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: "名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 名称  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "DestinationPath",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: "目标路径",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 目标路径  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<bool>(
                name: "Autonomous",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否自动修复",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: " 是否自动修复  ");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "EnumMemberDefinitions",
                type: "int",
                nullable: false,
                comment: "枚举值",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: " 枚举值  ");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumMemberDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: true,
                comment: "描述",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 描述  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "EnumDefinitions",
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
                name: "NameSpace",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: true,
                comment: "命名空间",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 命名空间  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: false,
                comment: "枚举名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 枚举名  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: true,
                comment: "前导",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 前导  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Using",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: "类引用的命名空间",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 类引用的命名空间  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "ClassDefinitions",
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
                name: "NameSpace",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: "命名空间",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 命名空间  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: "类名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " 类名  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: "前导",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 前导  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ClassDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Base",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: "基类或者接口名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 基类或者接口名称  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: false,
                comment: "Attribute文本",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " Attribute文本  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: false,
                comment: "Attribute名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: " Attribute名称  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AttributeDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "ArgumentsText",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: true,
                comment: "参数文本",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 参数文本  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Arguments",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: true,
                comment: "参数",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: " 参数  ")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnum",
                table: "PropertyDefinitions");

            migrationBuilder.DropColumn(
                name: "Remark",
                table: "PropertyDefinitions");

            migrationBuilder.AlterTable(
                name: "PropertyDefinitions",
                comment: " 类定义  ",
                oldComment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "Projects",
                comment: " 项目定义  ",
                oldComment: "项目定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "Maids",
                comment: " 功能  ",
                oldComment: "功能")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "EnumMemberDefinitions",
                comment: " 枚举成员定义  ",
                oldComment: "枚举成员定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "EnumDefinitions",
                comment: " 枚举定义  ",
                oldComment: "枚举定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "ClassDefinitions",
                comment: " 类定义  ",
                oldComment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "AttributeDefinitions",
                comment: " 类定义  ",
                oldComment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 数据类型  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "数据类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "PropertyDefinitions",
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
                name: "Set",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: " Set方法体  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "Set方法体")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 属性名称  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "属性名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Modifiers",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 修饰符  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "修饰符")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 前导  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "前导")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Initializer",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 初始化器  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "初始化器")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "HasSet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: " 是否包含Set  ",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否包含Set");

            migrationBuilder.AlterColumn<bool>(
                name: "HasGet",
                table: "PropertyDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: " 是否包含Get  ",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否包含Get");

            migrationBuilder.AlterColumn<string>(
                name: "Get",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: " Get方法体  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "Get方法体")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FullText",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 完整文本内容  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "完整文本内容")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "PropertyDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: " 项目路径  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "项目路径")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: " 项目名  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "项目名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Projects",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "GitBranch",
                table: "Projects",
                type: "longtext",
                nullable: false,
                comment: " Git分支  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "Git分支")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Projects",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "SourcePath",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: " 原路径  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "原路径")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: true,
                comment: " 序列化保存的设置  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "序列化保存的设置")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: " 名称  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "DestinationPath",
                table: "Maids",
                type: "longtext",
                nullable: false,
                comment: " 目标路径  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "目标路径")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "Maids",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<bool>(
                name: "Autonomous",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                comment: " 是否自动修复  ",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否自动修复");

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "EnumMemberDefinitions",
                type: "int",
                nullable: false,
                comment: " 枚举值  ",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "枚举值");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

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

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumMemberDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EnumMemberDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 描述  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "描述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumMemberDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "EnumDefinitions",
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
                name: "NameSpace",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 命名空间  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "命名空间")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 枚举名  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "枚举名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 前导  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "前导")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "EnumDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "EnumDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Using",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 类引用的命名空间  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "类引用的命名空间")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "ClassDefinitions",
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
                name: "NameSpace",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 命名空间  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "命名空间")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: " 类名  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "类名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 前导  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "前导")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ClassDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "ClassDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Base",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 基类或者接口名称  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "基类或者接口名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: false,
                comment: " Attribute文本  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "Attribute文本")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: false,
                comment: " Attribute名称  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "Attribute名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AttributeDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "AttributeDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "ArgumentsText",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 参数文本  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "参数文本")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Arguments",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: true,
                comment: " 参数  ",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "参数")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
