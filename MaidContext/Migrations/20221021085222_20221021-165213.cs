using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20221021165213 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maid_Project_ProjectId",
                table: "Maid");

            migrationBuilder.DropForeignKey(
                name: "FK_NameSpaceDefinitions_Maid_MaidId",
                table: "NameSpaceDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maid",
                table: "Maid");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "Maid",
                newName: "Maids");

            migrationBuilder.RenameIndex(
                name: "IX_Maid_ProjectId",
                table: "Maids",
                newName: "IX_Maids_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maids",
                table: "Maids",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Maids_Projects_ProjectId",
                table: "Maids",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NameSpaceDefinitions_Maids_MaidId",
                table: "NameSpaceDefinitions",
                column: "MaidId",
                principalTable: "Maids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maids_Projects_ProjectId",
                table: "Maids");

            migrationBuilder.DropForeignKey(
                name: "FK_NameSpaceDefinitions_Maids_MaidId",
                table: "NameSpaceDefinitions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Maids",
                table: "Maids");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameTable(
                name: "Maids",
                newName: "Maid");

            migrationBuilder.RenameIndex(
                name: "IX_Maids_ProjectId",
                table: "Maid",
                newName: "IX_Maid_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Maid",
                table: "Maid",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Maid_Project_ProjectId",
                table: "Maid",
                column: "ProjectId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NameSpaceDefinitions_Maid_MaidId",
                table: "NameSpaceDefinitions",
                column: "MaidId",
                principalTable: "Maid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
