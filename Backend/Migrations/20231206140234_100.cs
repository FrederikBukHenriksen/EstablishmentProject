using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "User",
                newName: "Email");

            migrationBuilder.RenameIndex(
                name: "IX_User_Username",
                table: "User",
                newName: "IX_User_Email");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "User",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                table: "User",
                newName: "IX_User_Username");
        }
    }
}
