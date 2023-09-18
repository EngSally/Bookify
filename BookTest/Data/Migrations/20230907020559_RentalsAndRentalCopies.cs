using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class RentalsAndRentalCopies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubscriberId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PenaltyPaid = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdateById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastUpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rentals_AspNetUsers_LastUpdateById",
                        column: x => x.LastUpdateById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Rentals_Subscribers_SubscriberId",
                        column: x => x.SubscriberId,
                        principalTable: "Subscribers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RentalCopies",
                columns: table => new
                {
                    RentalId = table.Column<int>(type: "int", nullable: false),
                    BookCopyId = table.Column<int>(type: "int", nullable: false),
                    RentalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtendedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalCopies", x => new { x.RentalId, x.BookCopyId });
                    table.ForeignKey(
                        name: "FK_RentalCopies_BooksCopies_BookCopyId",
                        column: x => x.BookCopyId,
                        principalTable: "BooksCopies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RentalCopies_Rentals_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Rentals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RentalCopies_BookCopyId",
                table: "RentalCopies",
                column: "BookCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_CreatedById",
                table: "Rentals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_LastUpdateById",
                table: "Rentals",
                column: "LastUpdateById");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_SubscriberId",
                table: "Rentals",
                column: "SubscriberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RentalCopies");

            migrationBuilder.DropTable(
                name: "Rentals");
        }
    }
}
