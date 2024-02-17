using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class OKAY1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Table_TableId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Table_Establishment_EstablishmentId",
                table: "Table");

            migrationBuilder.DropIndex(
                name: "IX_Sale_TableId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SaleType",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "PriceCurrency",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "PriceValue",
                table: "Item",
                newName: "Price");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Table",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Establishment",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SalesTables",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TableId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesTables_Sale_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesTables_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesTables_SaleId",
                table: "SalesTables",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesTables_TableId",
                table: "SalesTables",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Establishment_EstablishmentId",
                table: "Table",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Table_Establishment_EstablishmentId",
                table: "Table");

            migrationBuilder.DropTable(
                name: "SalesTables");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Item",
                newName: "PriceValue");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Table",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<int>(
                name: "PaymentType",
                table: "Sale",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SaleType",
                table: "Sale",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TableId",
                table: "Sale",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceCurrency",
                table: "Item",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Establishment",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_TableId",
                table: "Sale",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Table_TableId",
                table: "Sale",
                column: "TableId",
                principalTable: "Table",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Table_Establishment_EstablishmentId",
                table: "Table",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");
        }
    }
}
