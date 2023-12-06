using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDb.Migrations
{
    /// <inheritdoc />
    public partial class OrderHistoryFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_OrderHistories_OrderHistoryId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_ISBNs_OrderHistories_OrderHistoryId",
                table: "ISBNs");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookOrderHistory");

            migrationBuilder.DropTable(
                name: "CustomerOrderHistory");

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
    }
}
