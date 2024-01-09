using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _1000 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHours_information_InformationId",
                table: "OpeningHours");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Establishment_EstablishmentId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Item");

            migrationBuilder.RenameColumn(
                name: "InformationId",
                table: "OpeningHours",
                newName: "EstablishmentInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningHours_InformationId",
                table: "OpeningHours",
                newName: "IX_OpeningHours_EstablishmentInformationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Sale",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Sale",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Item",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstablishmentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Price",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PriceValue = table.Column<double>(type: "double precision", nullable: false),
                    PriceCurrency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Price", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Price_Item_Id",
                        column: x => x.Id,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_EmployeeId",
                table: "Sale",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_EstablishmentId",
                table: "Employee",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_information_EstablishmentInformationId",
                table: "OpeningHours",
                column: "EstablishmentInformationId",
                principalTable: "information",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Employee_EmployeeId",
                table: "Sale",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Establishment_EstablishmentId",
                table: "Sale",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHours_information_EstablishmentInformationId",
                table: "OpeningHours");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Employee_EmployeeId",
                table: "Sale");

            migrationBuilder.DropForeignKey(
                name: "FK_Sale_Establishment_EstablishmentId",
                table: "Sale");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Price");

            migrationBuilder.DropIndex(
                name: "IX_Sale_EmployeeId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "PaymentType",
                table: "Sale");

            migrationBuilder.DropColumn(
                name: "SaleType",
                table: "Sale");

            migrationBuilder.RenameColumn(
                name: "EstablishmentInformationId",
                table: "OpeningHours",
                newName: "InformationId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningHours_EstablishmentInformationId",
                table: "OpeningHours",
                newName: "IX_OpeningHours_InformationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Sale",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Item",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_information_InformationId",
                table: "OpeningHours",
                column: "InformationId",
                principalTable: "information",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sale_Establishment_EstablishmentId",
                table: "Sale",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
