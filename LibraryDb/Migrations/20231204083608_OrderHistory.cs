using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDb.Migrations
{
    /// <inheritdoc />
    public partial class OrderHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId",
                table: "ISBNs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderHistoryId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ISBNs_OrderHistoryId",
                table: "ISBNs",
                column: "OrderHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OrderHistoryId",
                table: "Customers",
                column: "OrderHistoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_OrderHistories_OrderHistoryId",
                table: "Customers",
                column: "OrderHistoryId",
                principalTable: "OrderHistories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ISBNs_OrderHistories_OrderHistoryId",
                table: "ISBNs",
                column: "OrderHistoryId",
                principalTable: "OrderHistories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_OrderHistories_OrderHistoryId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_ISBNs_OrderHistories_OrderHistoryId",
                table: "ISBNs");

            migrationBuilder.DropTable(
                name: "OrderHistories");

            migrationBuilder.DropIndex(
                name: "IX_ISBNs_OrderHistoryId",
                table: "ISBNs");

            migrationBuilder.DropIndex(
                name: "IX_Customers_OrderHistoryId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId",
                table: "ISBNs");

            migrationBuilder.DropColumn(
                name: "OrderHistoryId",
                table: "Customers");
        }
    }
}
