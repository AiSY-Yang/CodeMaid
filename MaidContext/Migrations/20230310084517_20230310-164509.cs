using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
    public partial class _20230310164509 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "IX_ClassDefinitions_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.AlterColumn<int>(
                name: "MaidWork",
                table: "Maids",
                type: "int",
                nullable: false,
                comment: "功能(0-配置同步功能,1-DTO同步,0-sasas,0-,1-test3,40-test4,41-test5,2-枚举remarks标签同步)",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "功能(0-配置同步功能,1-DTO同步,2-枚举remarks标签同步)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassDefinitions",
                type: "varchar(255)",
                nullable: false,
                comment: "类名",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "类名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "MaidId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions",
                columns: new[] { "Name", "MaidId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDefinitions_Maids_MaidId",
                table: "ClassDefinitions",
                column: "MaidId",
                principalTable: "Maids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassDefinitions_Maids_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ClassDefinitions_Name_MaidId",
                table: "ClassDefinitions");

            migrationBuilder.AlterColumn<int>(
                name: "MaidWork",
                table: "Maids",
                type: "int",
                nullable: false,
                comment: "功能(0-配置同步功能,1-DTO同步,2-枚举remarks标签同步)",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "功能(0-配置同步功能,1-DTO同步,0-sasas,0-,1-test3,40-test4,41-test5,2-枚举remarks标签同步)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ClassDefinitions",
                type: "longtext",
                nullable: false,
                comment: "类名",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "类名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "MaidId",
                table: "ClassDefinitions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassDefinitions_Maids_MaidId",
                table: "ClassDefinitions",
                column: "MaidId",
                principalTable: "Maids",
                principalColumn: "Id");
        }
    }
}
