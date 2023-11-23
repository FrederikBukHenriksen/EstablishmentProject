using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_Location_LocationId",
                table: "Establishment");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_LocationId",
                table: "Establishment");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Establishment");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Item",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_Name",
                table: "Item",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Item_Name",
                table: "Item");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Item",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Establishment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Establishment_LocationId",
                table: "Establishment",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_Location_LocationId",
                table: "Establishment",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
