using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_minkey",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_minkey",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "minkey",
                table: "Item");

            migrationBuilder.AddColumn<Guid>(
                name: "EstablishmentId",
                table: "Item",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_EstablishmentId",
                table: "Item",
                column: "EstablishmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
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

            migrationBuilder.DropIndex(
                name: "IX_Item_EstablishmentId",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "EstablishmentId",
                table: "Item");

            migrationBuilder.AddColumn<Guid>(
                name: "minkey",
                table: "Item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Item_minkey",
                table: "Item",
                column: "minkey");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_minkey",
                table: "Item",
                column: "minkey",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
