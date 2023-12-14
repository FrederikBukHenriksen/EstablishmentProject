using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _404 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentInformation_EstablishmentInforma~",
                table: "Establishment");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHours_EstablishmentInformation_EstablishmentInformat~",
                table: "OpeningHours");

            migrationBuilder.DropTable(
                name: "EstablishmentInformation");

            migrationBuilder.RenameColumn(
                name: "EstablishmentInformationId",
                table: "OpeningHours",
                newName: "InformationId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningHours_EstablishmentInformationId",
                table: "OpeningHours",
                newName: "IX_OpeningHours_InformationId");

            migrationBuilder.RenameColumn(
                name: "EstablishmentInformationId",
                table: "Establishment",
                newName: "InformationId");

            migrationBuilder.RenameIndex(
                name: "IX_Establishment_EstablishmentInformationId",
                table: "Establishment",
                newName: "IX_Establishment_InformationId");

            migrationBuilder.CreateTable(
                name: "information",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_information", x => x.Id);
                    table.ForeignKey(
                        name: "FK_information_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_information_LocationId",
                table: "information",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_information_InformationId",
                table: "Establishment",
                column: "InformationId",
                principalTable: "information",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_information_InformationId",
                table: "OpeningHours",
                column: "InformationId",
                principalTable: "information",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_information_InformationId",
                table: "Establishment");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHours_information_InformationId",
                table: "OpeningHours");

            migrationBuilder.DropTable(
                name: "information");

            migrationBuilder.RenameColumn(
                name: "InformationId",
                table: "OpeningHours",
                newName: "EstablishmentInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningHours_InformationId",
                table: "OpeningHours",
                newName: "IX_OpeningHours_EstablishmentInformationId");

            migrationBuilder.RenameColumn(
                name: "InformationId",
                table: "Establishment",
                newName: "EstablishmentInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_Establishment_InformationId",
                table: "Establishment",
                newName: "IX_Establishment_EstablishmentInformationId");

            migrationBuilder.CreateTable(
                name: "EstablishmentInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentInformation_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentInformation_LocationId",
                table: "EstablishmentInformation",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentInformation_EstablishmentInforma~",
                table: "Establishment",
                column: "EstablishmentInformationId",
                principalTable: "EstablishmentInformation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_EstablishmentInformation_EstablishmentInformat~",
                table: "OpeningHours",
                column: "EstablishmentInformationId",
                principalTable: "EstablishmentInformation",
                principalColumn: "Id");
        }
    }
}
