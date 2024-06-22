using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    /// <inheritdoc />
    public partial class _20240622220919 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "ProjectDirectoryFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Using",
                table: "ClassDefinitions",
                type: "text",
                nullable: true,
                comment: "类引用的命名空间",
                oldClrType: typeof(string),
                oldType: "text",
                oldComment: "类引用的命名空间");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDirectoryFiles_ProjectId",
                table: "ProjectDirectoryFiles",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDirectoryFiles_Projects_ProjectId",
                table: "ProjectDirectoryFiles",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDirectoryFiles_Projects_ProjectId",
                table: "ProjectDirectoryFiles");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDirectoryFiles_ProjectId",
                table: "ProjectDirectoryFiles");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectDirectoryFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Using",
                table: "ClassDefinitions",
                type: "text",
                nullable: false,
                defaultValue: "",
                comment: "类引用的命名空间",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldComment: "类引用的命名空间");
        }
    }
}
