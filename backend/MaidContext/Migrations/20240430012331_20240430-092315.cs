using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MaidContexts.Migrations
{
    /// <inheritdoc />
    public partial class _20240430092315 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectDirectories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "目录名"),
                    Path = table.Column<string>(type: "text", nullable: false, comment: "路径"),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDirectories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDirectories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "项目目录");

            migrationBuilder.CreateTable(
                name: "ProjectDirectoryFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false, comment: "文件名"),
                    Path = table.Column<string>(type: "text", nullable: false, comment: "路径"),
                    ProjectDirectoryId = table.Column<long>(type: "bigint", nullable: false),
                    IsAutoGen = table.Column<bool>(type: "boolean", nullable: false, comment: "是否是自动生成的文件"),
                    LinesCount = table.Column<int>(type: "integer", nullable: false, comment: "总行数"),
                    SpaceCount = table.Column<int>(type: "integer", nullable: false, comment: "空行数"),
                    CommentCount = table.Column<int>(type: "integer", nullable: false, comment: "注释行数"),
                    FileType = table.Column<int>(type: "integer", nullable: false, comment: "文件类型(0-其他,1-C#文件)"),
                    CreateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true, comment: "更新时间"),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, comment: "是否有效")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectDirectoryFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectDirectoryFiles_ProjectDirectories_ProjectDirectoryId",
                        column: x => x.ProjectDirectoryId,
                        principalTable: "ProjectDirectories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "项目文件");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDirectories_ProjectId",
                table: "ProjectDirectories",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDirectoryFiles_ProjectDirectoryId",
                table: "ProjectDirectoryFiles",
                column: "ProjectDirectoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectDirectoryFiles");

            migrationBuilder.DropTable(
                name: "ProjectDirectories");
        }
    }
}
