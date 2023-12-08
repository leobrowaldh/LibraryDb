using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDb.Migrations
{
    /// <inheritdoc />
    public partial class OrderHistoryForeignKeysAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookOrderHistory");

            migrationBuilder.DropTable(
                name: "CustomerOrderHistory");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "OrderHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OrderHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_BookId",
                table: "OrderHistories",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderHistories_CustomerId",
                table: "OrderHistories",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistories_Books_BookId",
                table: "OrderHistories",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHistories_Customers_CustomerId",
                table: "OrderHistories",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistories_Books_BookId",
                table: "OrderHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderHistories_Customers_CustomerId",
                table: "OrderHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrderHistories_BookId",
                table: "OrderHistories");

            migrationBuilder.DropIndex(
                name: "IX_OrderHistories_CustomerId",
                table: "OrderHistories");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "OrderHistories");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderHistories");

            migrationBuilder.CreateTable(
                name: "BookOrderHistory",
                columns: table => new
                {
                    BooksId = table.Column<int>(type: "int", nullable: false),
                    OrderHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookOrderHistory", x => new { x.BooksId, x.OrderHistoryId });
                    table.ForeignKey(
                        name: "FK_BookOrderHistory_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookOrderHistory_OrderHistories_OrderHistoryId",
                        column: x => x.OrderHistoryId,
                        principalTable: "OrderHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderHistory",
                columns: table => new
                {
                    CustomersId = table.Column<int>(type: "int", nullable: false),
                    OrderHistoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderHistory", x => new { x.CustomersId, x.OrderHistoryId });
                    table.ForeignKey(
                        name: "FK_CustomerOrderHistory_Customers_CustomersId",
                        column: x => x.CustomersId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerOrderHistory_OrderHistories_OrderHistoryId",
                        column: x => x.OrderHistoryId,
                        principalTable: "OrderHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookOrderHistory_OrderHistoryId",
                table: "BookOrderHistory",
                column: "OrderHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderHistory_OrderHistoryId",
                table: "CustomerOrderHistory",
                column: "OrderHistoryId");
        }
    }
}
