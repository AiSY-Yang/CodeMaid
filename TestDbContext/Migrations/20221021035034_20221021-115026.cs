using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221021115026 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "PropertyDefinitions",
                comment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "NameSpaceDefinitions",
                comment: "命名空间定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "ClassDefinitions",
                comment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "AttributeDefinitions",
                comment: "类定义")
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
                comment: "数据类型",
                oldClrType: typeof(string),
                oldType: "longtext")
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
                oldNullable: true)
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
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "属性名称",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Modifiers",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "修饰符",
                oldClrType: typeof(string),
                oldType: "longtext")
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
                oldNullable: true)
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
                comment: "初始化器",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Get",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                comment: "Get方法体",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "FullText",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "完整文本内容",
                oldClrType: typeof(string),
                oldType: "longtext")
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

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PropertyDefinitions",
                type: "bigint",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "NameSpaceDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NameSpaceDefinitions",
                type: "longtext",
                nullable: false,
                comment: "命名空间名称",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NameSpaceDefinitions",
                type: "tinyint(1)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "NameSpaceDefinitions",
                type: "datetime(6)",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "NameSpaceDefinitions",
                type: "bigint",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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
                comment: "注释",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: "类名",
                oldClrType: typeof(string),
                oldType: "longtext")
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
                comment: "基类或者接口名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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
                comment: "Attribute文本",
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AttributeDefinitions",
                type: "longtext",
                nullable: false,
                comment: "Attribute名称",
                oldClrType: typeof(string),
                oldType: "longtext")
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
                comment: "参数文本",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
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
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AttributeDefinitions",
                type: "bigint",
                nullable: false,
                comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>",
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "项目名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "longtext", nullable: true, comment: "项目路径")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                },
                comment: "项目定义")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.AlterTable(
                name: "PropertyDefinitions",
                oldComment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "NameSpaceDefinitions",
                oldComment: "命名空间定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "ClassDefinitions",
                oldComment: "类定义")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "AttributeDefinitions",
                oldComment: "类定义")
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
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Initializer",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "初始化器")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Get",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: true,
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
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "PropertyDefinitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdateTime",
                table: "NameSpaceDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "NameSpaceDefinitions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "命名空间名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "NameSpaceDefinitions",
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreateTime",
                table: "NameSpaceDefinitions",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetime(6)",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "NameSpaceDefinitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "注释")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "类名")
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
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "基类或者接口名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

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
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "参数")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "AttributeDefinitions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
