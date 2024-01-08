using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTest.Data.Migrations
{
	/// <inheritdoc />
	public partial class updatecolCreated : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "CretedOn",
				table: "Categories",
				newName: "CreatedOn");

			migrationBuilder.RenameColumn(
				name: "CretedOn",
				table: "Books",
				newName: "CreatedOn");

			migrationBuilder.RenameColumn(
				name: "CretedOn",
				table: "Authors",
				newName: "CreatedOn");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "CreatedOn",
				table: "Categories",
				newName: "CretedOn");

			migrationBuilder.RenameColumn(
				name: "CreatedOn",
				table: "Books",
				newName: "CretedOn");

			migrationBuilder.RenameColumn(
				name: "CreatedOn",
				table: "Authors",
				newName: "CretedOn");
		}
	}
}
