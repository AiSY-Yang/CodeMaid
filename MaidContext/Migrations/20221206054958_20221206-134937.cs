using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221206134937 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remark",
                table: "EnumDefinitions");

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

            migrationBuilder.AlterColumn<bool>(
                name: "Autonomous",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                comment: " 是否自动修复  ",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "是否自动修复");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<bool>(
                name: "Autonomous",
                table: "Maids",
                type: "tinyint(1)",
                nullable: false,
                comment: "是否自动修复",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: " 是否自动修复  ");

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

            migrationBuilder.AddColumn<string>(
                name: "Remark",
                table: "EnumDefinitions",
                type: "longtext",
                nullable: true,
                comment: "备注")
                .Annotation("MySql:CharSet", "utf8mb4");

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
    }
}
