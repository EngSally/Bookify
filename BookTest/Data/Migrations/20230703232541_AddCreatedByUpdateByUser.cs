using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddCreatedByUpdateByUser : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "LastUpdate",
				table: "Categories",
				newName: "LastUpdateOn");

			migrationBuilder.RenameColumn(
				name: "LastUpdate",
				table: "BooksCopies",
				newName: "LastUpdateOn");

			migrationBuilder.RenameColumn(
				name: "LastUpdate",
				table: "Books",
				newName: "LastUpdateOn");

			migrationBuilder.RenameColumn(
				name: "LastUpdate",
				table: "Authors",
				newName: "LastUpdateOn");

			migrationBuilder.RenameColumn(
				name: "LastUpdate",
				table: "AspNetUsers",
				newName: "LastUpdateOn");

			migrationBuilder.AddColumn<string>(
				name: "CreatedById",
				table: "Categories",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LastUpdateById",
				table: "Categories",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "CreatedById",
				table: "BooksCopies",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LastUpdateById",
				table: "BooksCopies",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "CreatedById",
				table: "Books",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LastUpdateById",
				table: "Books",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "CreatedById",
				table: "Authors",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LastUpdateById",
				table: "Authors",
				type: "nvarchar(450)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "CreatedById",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "LastUpdateById",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Categories_CreatedById",
				table: "Categories",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Categories_LastUpdateById",
				table: "Categories",
				column: "LastUpdateById");

			migrationBuilder.CreateIndex(
				name: "IX_BooksCopies_CreatedById",
				table: "BooksCopies",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_BooksCopies_LastUpdateById",
				table: "BooksCopies",
				column: "LastUpdateById");

			migrationBuilder.CreateIndex(
				name: "IX_Books_CreatedById",
				table: "Books",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Books_LastUpdateById",
				table: "Books",
				column: "LastUpdateById");

			migrationBuilder.CreateIndex(
				name: "IX_Authors_CreatedById",
				table: "Authors",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Authors_LastUpdateById",
				table: "Authors",
				column: "LastUpdateById");

			migrationBuilder.AddForeignKey(
				name: "FK_Authors_AspNetUsers_CreatedById",
				table: "Authors",
				column: "CreatedById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Authors_AspNetUsers_LastUpdateById",
				table: "Authors",
				column: "LastUpdateById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Books_AspNetUsers_CreatedById",
				table: "Books",
				column: "CreatedById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Books_AspNetUsers_LastUpdateById",
				table: "Books",
				column: "LastUpdateById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_BooksCopies_AspNetUsers_CreatedById",
				table: "BooksCopies",
				column: "CreatedById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_BooksCopies_AspNetUsers_LastUpdateById",
				table: "BooksCopies",
				column: "LastUpdateById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Categories_AspNetUsers_CreatedById",
				table: "Categories",
				column: "CreatedById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");

			migrationBuilder.AddForeignKey(
				name: "FK_Categories_AspNetUsers_LastUpdateById",
				table: "Categories",
				column: "LastUpdateById",
				principalTable: "AspNetUsers",
				principalColumn: "Id");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Authors_AspNetUsers_CreatedById",
				table: "Authors");

			migrationBuilder.DropForeignKey(
				name: "FK_Authors_AspNetUsers_LastUpdateById",
				table: "Authors");

			migrationBuilder.DropForeignKey(
				name: "FK_Books_AspNetUsers_CreatedById",
				table: "Books");

			migrationBuilder.DropForeignKey(
				name: "FK_Books_AspNetUsers_LastUpdateById",
				table: "Books");

			migrationBuilder.DropForeignKey(
				name: "FK_BooksCopies_AspNetUsers_CreatedById",
				table: "BooksCopies");

			migrationBuilder.DropForeignKey(
				name: "FK_BooksCopies_AspNetUsers_LastUpdateById",
				table: "BooksCopies");

			migrationBuilder.DropForeignKey(
				name: "FK_Categories_AspNetUsers_CreatedById",
				table: "Categories");

			migrationBuilder.DropForeignKey(
				name: "FK_Categories_AspNetUsers_LastUpdateById",
				table: "Categories");

			migrationBuilder.DropIndex(
				name: "IX_Categories_CreatedById",
				table: "Categories");

			migrationBuilder.DropIndex(
				name: "IX_Categories_LastUpdateById",
				table: "Categories");

			migrationBuilder.DropIndex(
				name: "IX_BooksCopies_CreatedById",
				table: "BooksCopies");

			migrationBuilder.DropIndex(
				name: "IX_BooksCopies_LastUpdateById",
				table: "BooksCopies");

			migrationBuilder.DropIndex(
				name: "IX_Books_CreatedById",
				table: "Books");

			migrationBuilder.DropIndex(
				name: "IX_Books_LastUpdateById",
				table: "Books");

			migrationBuilder.DropIndex(
				name: "IX_Authors_CreatedById",
				table: "Authors");

			migrationBuilder.DropIndex(
				name: "IX_Authors_LastUpdateById",
				table: "Authors");

			migrationBuilder.DropColumn(
				name: "CreatedById",
				table: "Categories");

			migrationBuilder.DropColumn(
				name: "LastUpdateById",
				table: "Categories");

			migrationBuilder.DropColumn(
				name: "CreatedById",
				table: "BooksCopies");

			migrationBuilder.DropColumn(
				name: "LastUpdateById",
				table: "BooksCopies");

			migrationBuilder.DropColumn(
				name: "CreatedById",
				table: "Books");

			migrationBuilder.DropColumn(
				name: "LastUpdateById",
				table: "Books");

			migrationBuilder.DropColumn(
				name: "CreatedById",
				table: "Authors");

			migrationBuilder.DropColumn(
				name: "LastUpdateById",
				table: "Authors");

			migrationBuilder.DropColumn(
				name: "CreatedById",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastUpdateById",
				table: "AspNetUsers");

			migrationBuilder.RenameColumn(
				name: "LastUpdateOn",
				table: "Categories",
				newName: "LastUpdate");

			migrationBuilder.RenameColumn(
				name: "LastUpdateOn",
				table: "BooksCopies",
				newName: "LastUpdate");

			migrationBuilder.RenameColumn(
				name: "LastUpdateOn",
				table: "Books",
				newName: "LastUpdate");

			migrationBuilder.RenameColumn(
				name: "LastUpdateOn",
				table: "Authors",
				newName: "LastUpdate");

			migrationBuilder.RenameColumn(
				name: "LastUpdateOn",
				table: "AspNetUsers",
				newName: "LastUpdate");
		}
	}
}
