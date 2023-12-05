using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryDb.Migrations
{
    /// <inheritdoc />
    public partial class InitialAzure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PIN = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "TheAuthors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheAuthors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    OrderHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customers_OrderHistories_OrderHistoryId",
                        column: x => x.OrderHistoryId,
                        principalTable: "OrderHistories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ISBNs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    OrderHistoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISBNs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ISBNs_OrderHistories_OrderHistoryId",
                        column: x => x.OrderHistoryId,
                        principalTable: "OrderHistories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Borrowed = table.Column<bool>(type: "bit", nullable: false),
                    BorrowedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ISBNId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Books_ISBNs_ISBNId",
                        column: x => x.ISBNId,
                        principalTable: "ISBNs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ISBNTheAuthor",
                columns: table => new
                {
                    AuthorsId = table.Column<int>(type: "int", nullable: false),
                    ISBNsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ISBNTheAuthor", x => new { x.AuthorsId, x.ISBNsId });
                    table.ForeignKey(
                        name: "FK_ISBNTheAuthor_ISBNs_ISBNsId",
                        column: x => x.ISBNsId,
                        principalTable: "ISBNs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ISBNTheAuthor_TheAuthors_AuthorsId",
                        column: x => x.AuthorsId,
                        principalTable: "TheAuthors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CustomerId",
                table: "Books",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBNId",
                table: "Books",
                column: "ISBNId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CardId",
                table: "Customers",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_OrderHistoryId",
                table: "Customers",
                column: "OrderHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ISBNs_OrderHistoryId",
                table: "ISBNs",
                column: "OrderHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ISBNTheAuthor_ISBNsId",
                table: "ISBNTheAuthor",
                column: "ISBNsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "ISBNTheAuthor");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ISBNs");

            migrationBuilder.DropTable(
                name: "TheAuthors");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "OrderHistories");
        }
    }
}
