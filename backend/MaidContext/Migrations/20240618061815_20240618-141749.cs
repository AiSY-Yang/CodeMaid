using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    /// <inheritdoc />
    public partial class _20240618141749 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStructure_ClassDefinitions_ClassDefinitionId",
                table: "ProjectStructure");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStructure_ProjectDirectoryFiles_ProjectDirectoryFile~",
                table: "ProjectStructure");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDefinitions_ProjectStructure_ProjectStructureId",
                table: "PropertyDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStructure",
                table: "ProjectStructure");

            migrationBuilder.RenameTable(
                name: "ProjectStructure",
                newName: "ProjectStructures");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStructure_ProjectDirectoryFileId",
                table: "ProjectStructures",
                newName: "IX_ProjectStructures_ProjectDirectoryFileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStructure_ClassDefinitionId",
                table: "ProjectStructures",
                newName: "IX_ProjectStructures_ClassDefinitionId");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: true,
                comment: "所属项目",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStructures",
                table: "ProjectStructures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStructures_ClassDefinitions_ClassDefinitionId",
                table: "ProjectStructures",
                column: "ClassDefinitionId",
                principalTable: "ClassDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStructures_ProjectDirectoryFiles_ProjectDirectoryFil~",
                table: "ProjectStructures",
                column: "ProjectDirectoryFileId",
                principalTable: "ProjectDirectoryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDefinitions_ProjectStructures_ProjectStructureId",
                table: "PropertyDefinitions",
                column: "ProjectStructureId",
                principalTable: "ProjectStructures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStructures_ClassDefinitions_ClassDefinitionId",
                table: "ProjectStructures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStructures_ProjectDirectoryFiles_ProjectDirectoryFil~",
                table: "ProjectStructures");

            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDefinitions_ProjectStructures_ProjectStructureId",
                table: "PropertyDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStructures",
                table: "ProjectStructures");

            migrationBuilder.RenameTable(
                name: "ProjectStructures",
                newName: "ProjectStructure");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStructures_ProjectDirectoryFileId",
                table: "ProjectStructure",
                newName: "IX_ProjectStructure_ProjectDirectoryFileId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectStructures_ClassDefinitionId",
                table: "ProjectStructure",
                newName: "IX_ProjectStructure_ClassDefinitionId");

            migrationBuilder.AlterColumn<long>(
                name: "ProjectId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "所属项目");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStructure",
                table: "ProjectStructure",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStructure_ClassDefinitions_ClassDefinitionId",
                table: "ProjectStructure",
                column: "ClassDefinitionId",
                principalTable: "ClassDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStructure_ProjectDirectoryFiles_ProjectDirectoryFile~",
                table: "ProjectStructure",
                column: "ProjectDirectoryFileId",
                principalTable: "ProjectDirectoryFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDefinitions_ProjectStructure_ProjectStructureId",
                table: "PropertyDefinitions",
                column: "ProjectStructureId",
                principalTable: "ProjectStructure",
                principalColumn: "Id");
        }
    }
}
