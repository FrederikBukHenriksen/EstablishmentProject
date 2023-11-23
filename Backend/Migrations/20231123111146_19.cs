using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SaleId",
                table: "Item",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstablishmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Table_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstablishmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TimestampStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TimestampEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TableId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_SaleId",
                table: "Item",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EstablishmentId",
                table: "Sale",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_TableId",
                table: "Sale",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_Table_EstablishmentId",
                table: "Table",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Sale_SaleId",
                table: "Item",
                column: "SaleId",
                principalTable: "Sale",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Sale_SaleId",
                table: "Item");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropIndex(
                name: "IX_Item_SaleId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "SaleId",
                table: "Item");
        }
    }
}
