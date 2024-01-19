using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class addSubscriber_Areas_Governorates : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Governorates",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Deleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdateById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					LastUpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Governorates", x => x.Id);
					table.ForeignKey(
						name: "FK_Governorates_AspNetUsers_CreatedById",
						column: x => x.CreatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Governorates_AspNetUsers_LastUpdateById",
						column: x => x.LastUpdateById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
				});

			migrationBuilder.CreateTable(
				name: "Areas",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					GovernorateId = table.Column<int>(type: "int", nullable: false),
					Deleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdateById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					LastUpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Areas", x => x.Id);
					table.ForeignKey(
						name: "FK_Areas_AspNetUsers_CreatedById",
						column: x => x.CreatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Areas_AspNetUsers_LastUpdateById",
						column: x => x.LastUpdateById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Areas_Governorates_GovernorateId",
						column: x => x.GovernorateId,
						principalTable: "Governorates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Subscribers",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					FristName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
					NationalId = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
					MobilNum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
					HasWhatsApp = table.Column<bool>(type: "bit", nullable: false),
					Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
					ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
					ImageUrlThumbnail = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
					Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsBlackListed = table.Column<bool>(type: "bit", nullable: false),
					AreaId = table.Column<int>(type: "int", nullable: false),
					GovernorateId = table.Column<int>(type: "int", nullable: false),
					Deleted = table.Column<bool>(type: "bit", nullable: false),
					CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdateById = table.Column<string>(type: "nvarchar(450)", nullable: true),
					LastUpdateOn = table.Column<DateTime>(type: "datetime2", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Subscribers", x => x.Id);
					table.ForeignKey(
						name: "FK_Subscribers_Areas_AreaId",
						column: x => x.AreaId,
						principalTable: "Areas",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Subscribers_AspNetUsers_CreatedById",
						column: x => x.CreatedById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Subscribers_AspNetUsers_LastUpdateById",
						column: x => x.LastUpdateById,
						principalTable: "AspNetUsers",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_Subscribers_Governorates_GovernorateId",
						column: x => x.GovernorateId,
						principalTable: "Governorates",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Areas_CreatedById",
				table: "Areas",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Areas_GovernorateId",
				table: "Areas",
				column: "GovernorateId");

			migrationBuilder.CreateIndex(
				name: "IX_Areas_LastUpdateById",
				table: "Areas",
				column: "LastUpdateById");

			migrationBuilder.CreateIndex(
				name: "IX_Areas_Name_GovernorateId",
				table: "Areas",
				columns: new[] { "Name", "GovernorateId" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Governorates_CreatedById",
				table: "Governorates",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Governorates_LastUpdateById",
				table: "Governorates",
				column: "LastUpdateById");

			migrationBuilder.CreateIndex(
				name: "IX_Governorates_Name",
				table: "Governorates",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_AreaId",
				table: "Subscribers",
				column: "AreaId");

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_CreatedById",
				table: "Subscribers",
				column: "CreatedById");

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_Email",
				table: "Subscribers",
				column: "Email",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_GovernorateId",
				table: "Subscribers",
				column: "GovernorateId");

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_LastUpdateById",
				table: "Subscribers",
				column: "LastUpdateById");

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_MobilNum",
				table: "Subscribers",
				column: "MobilNum",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Subscribers_NationalId",
				table: "Subscribers",
				column: "NationalId",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Subscribers");

			migrationBuilder.DropTable(
				name: "Areas");

			migrationBuilder.DropTable(
				name: "Governorates");
		}
	}
}
