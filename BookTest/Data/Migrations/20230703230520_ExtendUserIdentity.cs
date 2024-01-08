using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTest.Data.Migrations
{
	/// <inheritdoc />
	public partial class ExtendUserIdentity : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedOn",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<bool>(
				name: "Deleted",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<string>(
				name: "FullName",
				table: "AspNetUsers",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<DateTime>(
				name: "LastUpdate",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "CreatedOn",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "Deleted",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "FullName",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastUpdate",
				table: "AspNetUsers");
		}
	}
}
