using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221202105532 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: true,
                comment: "序列化保存的设置",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
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
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnumDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NameSpace = table.Column<string>(type: "longtext", nullable: true, comment: "命名空间")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "枚举名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Summary = table.Column<string>(type: "longtext", nullable: true, comment: "注释")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "longtext", nullable: true, comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LeadingTrivia = table.Column<string>(type: "longtext", nullable: true, comment: "前导")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MaidId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumDefinitions_Maids_MaidId",
                        column: x => x.MaidId,
                        principalTable: "Maids",
                        principalColumn: "Id");
                },
                comment: "枚举定义")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnumMemberDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.Id\"/>")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false, comment: "枚举名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<int>(type: "int", nullable: false, comment: "枚举值"),
                    Summary = table.Column<string>(type: "longtext", nullable: true, comment: "注释")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnumDefinitionId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.CreateTime\"/>"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.UpdateTime\"/>"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "<inheritdoc cref=\"IDatabaseEntity.IsDeleted\"/>")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnumMemberDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnumMemberDefinitions_EnumDefinitions_EnumDefinitionId",
                        column: x => x.EnumDefinitionId,
                        principalTable: "EnumDefinitions",
                        principalColumn: "Id");
                },
                comment: "枚举成员定义")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EnumDefinitions_MaidId",
                table: "EnumDefinitions",
                column: "MaidId");

            migrationBuilder.CreateIndex(
                name: "IX_EnumMemberDefinitions_EnumDefinitionId",
                table: "EnumMemberDefinitions",
                column: "EnumDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnumMemberDefinitions");

            migrationBuilder.DropTable(
                name: "EnumDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Maids",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "序列化保存的设置")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "LeadingTrivia",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "前导")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
