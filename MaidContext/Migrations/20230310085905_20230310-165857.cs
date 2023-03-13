using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20230310165857 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyDefinitions",
                type: "varchar(255)",
                nullable: false,
                comment: "属性名称",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "属性名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDefinitions_Name_ClassDefinitionId",
                table: "PropertyDefinitions",
                columns: new[] { "Name", "ClassDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions",
                columns: new[] { "Name", "MaidId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyDefinitions_Name_ClassDefinitionId",
                table: "PropertyDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PropertyDefinitions",
                type: "longtext",
                nullable: false,
                comment: "属性名称",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "属性名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions",
                columns: new[] { "Name", "MaidId" });
        }
    }
}
