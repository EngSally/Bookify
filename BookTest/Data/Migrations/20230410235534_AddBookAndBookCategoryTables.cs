using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddBookAndBookCategoryTables : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropPrimaryKey(
				name: "PK_categories",
				table: "categories");

			migrationBuilder.DropPrimaryKey(
				name: "PK_authors",
				table: "authors");

			migrationBuilder.RenameTable(
				name: "categories",
				newName: "Categories");

			migrationBuilder.RenameTable(
				name: "authors",
				newName: "Authors");

			migrationBuilder.RenameIndex(
				name: "IX_categories_Name",
				table: "Categories",
				newName: "IX_Categories_Name");

			migrationBuilder.RenameIndex(
				name: "IX_authors_Name",
				table: "Authors",
				newName: "IX_Authors_Name");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Categories",
				table: "Categories",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_Authors",
				table: "Authors",
				column: "Id");

			migrationBuilder.CreateTable(
				name: "Books",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Titel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
					AuthorId = table.Column<int>(type: "int", nullable: false),
					Publisher = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
					PublisherDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Hall = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsAvailableForRental = table.Column<bool>(type: "bit", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Deleted = table.Column<bool>(type: "bit", nullable: false),
					CretedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Books", x => x.Id);
					table.ForeignKey(
						name: "FK_Books_Authors_AuthorId",
						column: x => x.AuthorId,
						principalTable: "Authors",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "BookCategory",
				columns: table => new
				{
					BooksId = table.Column<int>(type: "int", nullable: false),
					CategoriesId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BookCategory", x => new { x.BooksId, x.CategoriesId });
					table.ForeignKey(
						name: "FK_BookCategory_Books_BooksId",
						column: x => x.BooksId,
						principalTable: "Books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_BookCategory_Categories_CategoriesId",
						column: x => x.CategoriesId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "BooksCategories",
				columns: table => new
				{
					BookId = table.Column<int>(type: "int", nullable: false),
					CategoryId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BooksCategories", x => new { x.CategoryId, x.BookId });
					table.ForeignKey(
						name: "FK_BooksCategories_Books_BookId",
						column: x => x.BookId,
						principalTable: "Books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_BooksCategories_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_BookCategory_CategoriesId",
				table: "BookCategory",
				column: "CategoriesId");

			migrationBuilder.CreateIndex(
				name: "IX_Books_AuthorId",
				table: "Books",
				column: "AuthorId");

			migrationBuilder.CreateIndex(
				name: "IX_Books_Titel_AuthorId",
				table: "Books",
				columns: new[] { "Titel", "AuthorId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_BooksCategories_BookId",
				table: "BooksCategories",
				column: "BookId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "BookCategory");

			migrationBuilder.DropTable(
				name: "BooksCategories");

			migrationBuilder.DropTable(
				name: "Books");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Categories",
				table: "Categories");

			migrationBuilder.DropPrimaryKey(
				name: "PK_Authors",
				table: "Authors");

			migrationBuilder.RenameTable(
				name: "Categories",
				newName: "categories");

			migrationBuilder.RenameTable(
				name: "Authors",
				newName: "authors");

			migrationBuilder.RenameIndex(
				name: "IX_Categories_Name",
				table: "categories",
				newName: "IX_categories_Name");

			migrationBuilder.RenameIndex(
				name: "IX_Authors_Name",
				table: "authors",
				newName: "IX_authors_Name");

			migrationBuilder.AddPrimaryKey(
				name: "PK_categories",
				table: "categories",
				column: "Id");

			migrationBuilder.AddPrimaryKey(
				name: "PK_authors",
				table: "authors",
				column: "Id");
		}
	}
}
