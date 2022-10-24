using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221024102442 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeDefinitions_PropertyDefinitions_PropertyDefinitionId",
                table: "AttributeDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassDefinitions_NameSpaceDefinitions_NameSpaceDefinitionId",
                table: "ClassDefinitions");

            migrationBuilder.DropTable(
                name: "NameSpaceDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ClassDefinitions_NameSpaceDefinitionId",
                table: "ClassDefinitions");

            migrationBuilder.DropColumn(
                name: "NameSpaceDefinitionId",
                table: "ClassDefinitions");

            migrationBuilder.AddColumn<long>(
                name: "MaidId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSpace",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "PropertyDefinitionId",
                table: "AttributeDefinitions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_MaidId",
                table: "ClassDefinitions",
                column: "MaidId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeDefinitions_PropertyDefinitions_PropertyDefinitionId",
                table: "AttributeDefinitions",
                column: "PropertyDefinitionId",
                principalTable: "PropertyDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDefinitions_Maids_MaidId",
                table: "ClassDefinitions",
                column: "MaidId",
                principalTable: "Maids",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttributeDefinitions_PropertyDefinitions_PropertyDefinitionId",
                table: "AttributeDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassDefinitions_Maids_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ClassDefinitions_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.DropColumn(
                name: "MaidId",
                table: "ClassDefinitions");

            migrationBuilder.DropColumn(
                name: "NameSpace",
                table: "ClassDefinitions");

            migrationBuilder.AddColumn<long>(
                name: "NameSpaceDefinitionId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "PropertyDefinitionId",
                table: "AttributeDefinitions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "NameSpaceDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaidId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameSpaceDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NameSpaceDefinitions_Maids_MaidId",
                        column: x => x.MaidId,
                        principalTable: "Maids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_NameSpaceDefinitionId",
                table: "ClassDefinitions",
                column: "NameSpaceDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_NameSpaceDefinitions_MaidId",
                table: "NameSpaceDefinitions",
                column: "MaidId");

            migrationBuilder.AddForeignKey(
                name: "FK_AttributeDefinitions_PropertyDefinitions_PropertyDefinitionId",
                table: "AttributeDefinitions",
                column: "PropertyDefinitionId",
                principalTable: "PropertyDefinitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDefinitions_NameSpaceDefinitions_NameSpaceDefinitionId",
                table: "ClassDefinitions",
                column: "NameSpaceDefinitionId",
                principalTable: "NameSpaceDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
