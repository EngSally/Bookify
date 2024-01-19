using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class updatecategory : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Books_Categories_CategoryId",
				table: "Books");

			migrationBuilder.DropIndex(
				name: "IX_Books_CategoryId",
				table: "Books");

			migrationBuilder.DropColumn(
				name: "CategoryId",
				table: "Books");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "CategoryId",
				table: "Books",
				type: "int",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Books_CategoryId",
				table: "Books",
				column: "CategoryId");

			migrationBuilder.AddForeignKey(
				name: "FK_Books_Categories_CategoryId",
				table: "Books",
				column: "CategoryId",
				principalTable: "Categories",
				principalColumn: "Id");
		}
	}
}
