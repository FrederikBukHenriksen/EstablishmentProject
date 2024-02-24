using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class hello69 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentInformation_InformationId",
                table: "Establishment");

            migrationBuilder.DropTable(
                name: "EstablishmentInformation");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_InformationId",
                table: "Establishment");

            migrationBuilder.DropColumn(
                name: "InformationId",
                table: "Establishment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InformationId",
                table: "Establishment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentInformation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishment_InformationId",
                table: "Establishment",
                column: "InformationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentInformation_LocationId",
                table: "EstablishmentInformation",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentInformation_InformationId",
                table: "Establishment",
                column: "InformationId",
                principalTable: "EstablishmentInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
