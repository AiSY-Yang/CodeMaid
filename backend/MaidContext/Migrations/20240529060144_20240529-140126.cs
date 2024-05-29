using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    /// <inheritdoc />
    public partial class _20240529140126 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastWriteTime",
                table: "ProjectDirectoryFiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastWriteTime",
                table: "ProjectDirectoryFiles");
        }
    }
}
