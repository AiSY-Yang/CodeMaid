using System.Text.Json;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaidContexts.Migrations
{
	/// <inheritdoc />
	public partial class _20240109100653 : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("ALTER TABLE \"Maids\" ALTER COLUMN \"Setting\" TYPE jsonb USING \"Setting\"::jsonb\r\n");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Setting",
				table: "Maids",
				type: "text",
				nullable: false,
				comment: "设置",
				oldClrType: typeof(JsonDocument),
				oldType: "jsonb",
				oldComment: "设置");
		}
	}
}
