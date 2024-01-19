using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
	/// <inheritdoc />
	public partial class update_RenewalSubscribtionTable : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_RenewalSubscribtions_Subscribers_SubscriberId",
				table: "RenewalSubscribtions");

			migrationBuilder.DropColumn(
				name: "SubscribrId",
				table: "RenewalSubscribtions");

			migrationBuilder.AlterColumn<int>(
				name: "SubscriberId",
				table: "RenewalSubscribtions",
				type: "int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(int),
				oldType: "int",
				oldNullable: true);

			migrationBuilder.AddForeignKey(
				name: "FK_RenewalSubscribtions_Subscribers_SubscriberId",
				table: "RenewalSubscribtions",
				column: "SubscriberId",
				principalTable: "Subscribers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_RenewalSubscribtions_Subscribers_SubscriberId",
				table: "RenewalSubscribtions");

			migrationBuilder.AlterColumn<int>(
				name: "SubscriberId",
				table: "RenewalSubscribtions",
				type: "int",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");

			migrationBuilder.AddColumn<int>(
				name: "SubscribrId",
				table: "RenewalSubscribtions",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.AddForeignKey(
				name: "FK_RenewalSubscribtions_Subscribers_SubscriberId",
				table: "RenewalSubscribtions",
				column: "SubscriberId",
				principalTable: "Subscribers",
				principalColumn: "Id");
		}
	}
}
