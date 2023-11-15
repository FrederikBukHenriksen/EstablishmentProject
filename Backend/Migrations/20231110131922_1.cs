using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Establishment",
                columns: new[] { "Id", "LocationId", "Name" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), null, "My Establishment" });

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "EstablishmentId", "Name", "Price", "SaleId" },
                values: new object[,]
                {
                    { new Guid("386384d0-6d1f-456b-b866-89fca7d16fda"), null, "Espresso", 30f, null },
                    { new Guid("780ecbca-2005-4f24-8179-2051cf0a72b8"), null, "Bun with cheese", 60f, null },
                    { new Guid("eff1012e-e9df-42ab-b3c7-18e0f692ef1d"), null, "Smoothie", 50f, null }
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "EstablishmentId", "UserId", "Id", "Role" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("3947cd55-f7c5-4968-a33c-986300642816"), "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DeleteData(
                table: "UserRole",
                keyColumns: new[] { "EstablishmentId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                table: "Establishment",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));
        }
    }
}
