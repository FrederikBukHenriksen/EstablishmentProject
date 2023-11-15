using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item");

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumn: "Id",
                keyValue: new Guid("386384d0-6d1f-456b-b866-89fca7d16fda"));

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumn: "Id",
                keyValue: new Guid("780ecbca-2005-4f24-8179-2051cf0a72b8"));

            migrationBuilder.DeleteData(
                table: "Item",
                keyColumn: "Id",
                keyValue: new Guid("eff1012e-e9df-42ab-b3c7-18e0f692ef1d"));

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "EstablishmentId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") },
                column: "Id",
                value: new Guid("a9af66c5-9356-4714-91b6-e285110450c0"));

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Item",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "EstablishmentId", "Name", "Price", "SaleId" },
                values: new object[,]
                {
                    { new Guid("386384d0-6d1f-456b-b866-89fca7d16fda"), null, "Espresso", 30f, null },
                    { new Guid("780ecbca-2005-4f24-8179-2051cf0a72b8"), null, "Bun with cheese", 60f, null },
                    { new Guid("eff1012e-e9df-42ab-b3c7-18e0f692ef1d"), null, "Smoothie", 50f, null }
                });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "EstablishmentId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") },
                column: "Id",
                value: new Guid("3947cd55-f7c5-4968-a33c-986300642816"));

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");
        }
    }
}
