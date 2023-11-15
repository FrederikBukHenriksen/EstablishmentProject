﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item");

            migrationBuilder.AlterColumn<Guid>(
                name: "EstablishmentId",
                table: "Item",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Establishment_EstablishmentId",
                table: "Item",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");
        }
    }
}
