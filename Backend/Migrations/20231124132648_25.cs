﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EstablishmentId1",
                table: "Item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Item_EstablishmentId1",
                table: "Item",
                column: "EstablishmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId1",
                table: "Item",
                column: "EstablishmentId1",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId1",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_EstablishmentId1",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "EstablishmentId1",
                table: "Item");
        }
    }
}
