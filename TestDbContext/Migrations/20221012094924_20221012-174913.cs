using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221012174913 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FieldName",
                table: "FieldDefinitions",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "FullText",
                table: "FieldDefinitions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Get",
                table: "FieldDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Initializer",
                table: "FieldDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LeadingTrivia",
                table: "FieldDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Modifiers",
                table: "FieldDefinitions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Set",
                table: "FieldDefinitions",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Base",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullText",
                table: "FieldDefinitions");

            migrationBuilder.DropColumn(
                name: "Get",
                table: "FieldDefinitions");

            migrationBuilder.DropColumn(
                name: "Initializer",
                table: "FieldDefinitions");

            migrationBuilder.DropColumn(
                name: "LeadingTrivia",
                table: "FieldDefinitions");

            migrationBuilder.DropColumn(
                name: "Modifiers",
                table: "FieldDefinitions");

            migrationBuilder.DropColumn(
                name: "Set",
                table: "FieldDefinitions");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FieldDefinitions",
                newName: "FieldName");

            migrationBuilder.UpdateData(
                table: "ClassDefinitions",
                keyColumn: "Base",
                keyValue: null,
                column: "Base",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Base",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
