using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Infrastructure.Persistence.Migrations
{
	/// <inheritdoc />
	public partial class AddBookCopyTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "Shared");

			migrationBuilder.CreateSequence<int>(
				name: "SerialNumberSequance",
				schema: "Shared",
				startValue: 10000L);

			migrationBuilder.CreateTable(
				name: "BooksCopies",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					BookId = table.Column<int>(type: "int", nullable: false),
					IsAvailableForRental = table.Column<bool>(type: "bit", nullable: false),
					EditionNumber = table.Column<int>(type: "int", nullable: false),
					SerialNumber = table.Column<int>(type: "int", nullable: false, defaultValueSql: "Next  Value For Shared.SerialNumberSequance"),
					Deleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BooksCopies", x => x.Id);
					table.ForeignKey(
						name: "FK_BooksCopies_Books_BookId",
						column: x => x.BookId,
						principalTable: "Books",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_BooksCopies_BookId",
				table: "BooksCopies",
				column: "BookId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "BooksCopies");

			migrationBuilder.DropSequence(
				name: "SerialNumberSequance",
				schema: "Shared");
		}
	}
}
