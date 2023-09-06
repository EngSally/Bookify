using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateTitleColName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titel",
                table: "Books",
                newName: "Title");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Titel_AuthorId",
                table: "Books",
                newName: "IX_Books_Title_AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Books",
                newName: "Titel");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Title_AuthorId",
                table: "Books",
                newName: "IX_Books_Titel_AuthorId");
        }
    }
}
