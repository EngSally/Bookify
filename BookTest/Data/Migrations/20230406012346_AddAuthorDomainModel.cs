using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddAuthorDomainModel : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "authors",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Deleted = table.Column<bool>(type: "bit", nullable: false),
					CretedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_authors", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_authors_Name",
				table: "authors",
				column: "Name",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "authors");
		}
	}
}
