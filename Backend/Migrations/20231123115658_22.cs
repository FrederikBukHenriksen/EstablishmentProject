using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Sale_SaleId",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_SaleId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Item");

            migrationBuilder.CreateTable(
                name: "SalesItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesItems_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesItems_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_ItemId",
                table: "SalesItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_SaleId",
                table: "SalesItems",
                column: "SaleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalesItems");

            migrationBuilder.AddColumn<Guid>(
                name: "SaleId",
                table: "Item",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_SaleId",
                table: "Item",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Sale_SaleId",
                table: "Item",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id");
        }
    }
}
