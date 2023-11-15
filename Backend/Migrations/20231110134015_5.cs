using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Item",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "Item",
                columns: new[] { "Id", "EstablishmentId", "Name", "Price", "SaleId" },
                values: new object[] { new Guid("d054fe7d-18c8-49af-b192-888718d6c43d"), new Guid("00000000-0000-0000-0000-000000000001"), "Espresso", 30.0, null });

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "EstablishmentId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") },
                column: "Id",
                value: new Guid("37786532-a98f-46e3-9f53-6deef5c9db90"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Item",
                keyColumn: "Id",
                keyValue: new Guid("d054fe7d-18c8-49af-b192-888718d6c43d"));

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Item",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.UpdateData(
                table: "UserRole",
                keyColumns: new[] { "EstablishmentId", "UserId" },
                keyValues: new object[] { new Guid("00000000-0000-0000-0000-000000000001"), new Guid("00000000-0000-0000-0000-000000000001") },
                column: "Id",
                value: new Guid("a9af66c5-9356-4714-91b6-e285110450c0"));
        }
    }
}
