using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class addColImagePublicIdToBookTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrlPublicId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrlPublicId",
                table: "Books");
        }
    }
}
