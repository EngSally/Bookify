using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTest.Data.Migrations
{
	/// <inheritdoc />
	public partial class updtaeColNamePublishing : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "PublisherDate",
				table: "Books",
				newName: "PublishingDate");

			migrationBuilder.AlterColumn<string>(
				name: "Publisher",
				table: "Books",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "Hall",
				table: "Books",
				type: "nvarchar(50)",
				maxLength: 50,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "PublishingDate",
				table: "Books",
				newName: "PublisherDate");

			migrationBuilder.AlterColumn<string>(
				name: "Publisher",
				table: "Books",
				type: "nvarchar(200)",
				maxLength: 200,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(200)",
				oldMaxLength: 200);

			migrationBuilder.AlterColumn<string>(
				name: "Hall",
				table: "Books",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(50)",
				oldMaxLength: 50);
		}
	}
}
