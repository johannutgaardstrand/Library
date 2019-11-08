using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Custumers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    DamageToBooks = table.Column<int>(nullable: false),
                    ParentCustomerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custumers", x => x.CustomerID);
                    table.ForeignKey(
                        name: "FK_Custumers_Custumers_ParentCustomerID",
                        column: x => x.ParentCustomerID,
                        principalTable: "Custumers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    HallID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.HallID);
                });

            migrationBuilder.CreateTable(
                name: "WasteLists",
                columns: table => new
                {
                    WasteListID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteLists", x => x.WasteListID);
                });

            migrationBuilder.CreateTable(
                name: "Bills",
                columns: table => new
                {
                    BillID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillDate = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    AmountPayed = table.Column<double>(nullable: false),
                    CustumerID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bills", x => x.BillID);
                    table.ForeignKey(
                        name: "FK_Bills_Custumers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Custumers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shelves",
                columns: table => new
                {
                    ShelfID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShelfNumber = table.Column<int>(nullable: false),
                    HallID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelves", x => x.ShelfID);
                    table.ForeignKey(
                        name: "FK_Shelves_Halls_HallID",
                        column: x => x.HallID,
                        principalTable: "Halls",
                        principalColumn: "HallID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    DateOfPurchase = table.Column<DateTime>(nullable: false),
                    Condition = table.Column<byte>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    ISBN = table.Column<long>(nullable: false),
                    ShelfID = table.Column<int>(nullable: false),
                    WasteListID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Books_Shelves_ShelfID",
                        column: x => x.ShelfID,
                        principalTable: "Shelves",
                        principalColumn: "ShelfID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_WasteLists_WasteListID",
                        column: x => x.WasteListID,
                        principalTable: "WasteLists",
                        principalColumn: "WasteListID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Borrows",
                columns: table => new
                {
                    BorrowID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfBorrow = table.Column<DateTime>(nullable: false),
                    BookReturned = table.Column<bool>(nullable: false),
                    BookConditionDecreased = table.Column<byte>(nullable: false),
                    CustumerID = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: true),
                    BookID = table.Column<int>(nullable: false),
                    BillID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borrows", x => x.BorrowID);
                    table.ForeignKey(
                        name: "FK_Borrows_Bills_BillID",
                        column: x => x.BillID,
                        principalTable: "Bills",
                        principalColumn: "BillID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrows_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Borrows_Custumers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Custumers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_CustomerID",
                table: "Bills",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ShelfID",
                table: "Books",
                column: "ShelfID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_WasteListID",
                table: "Books",
                column: "WasteListID");

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BillID",
                table: "Borrows",
                column: "BillID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_BookID",
                table: "Borrows",
                column: "BookID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrows_CustomerID",
                table: "Borrows",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Custumers_ParentCustomerID",
                table: "Custumers",
                column: "ParentCustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_HallID",
                table: "Shelves",
                column: "HallID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrows");

            migrationBuilder.DropTable(
                name: "Bills");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Custumers");

            migrationBuilder.DropTable(
                name: "Shelves");

            migrationBuilder.DropTable(
                name: "WasteLists");

            migrationBuilder.DropTable(
                name: "Halls");
        }
    }
}
