using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _400 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_Location_LocationId",
                table: "Establishment");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHours_Establishment_EstablishmentId",
                table: "OpeningHours");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_LocationId",
                table: "Establishment");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Establishment");

            migrationBuilder.RenameColumn(
                name: "EstablishmentId",
                table: "OpeningHours",
                newName: "EstablishmentInformationId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningHours_EstablishmentId",
                table: "OpeningHours",
                newName: "IX_OpeningHours_EstablishmentInformationId");

            migrationBuilder.AddColumn<Guid>(
                name: "EstablishmentInformationId",
                table: "Establishment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                name: "IX_Establishment_EstablishmentInformationId",
                table: "Establishment",
                column: "EstablishmentInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentInformation_LocationId",
                table: "EstablishmentInformation",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentInformation_EstablishmentInforma~",
                table: "Establishment",
                column: "EstablishmentInformationId",
                principalTable: "EstablishmentInformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_EstablishmentInformation_EstablishmentInformat~",
                table: "OpeningHours",
                column: "EstablishmentInformationId",
                principalTable: "EstablishmentInformation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentInformation_EstablishmentInforma~",
                table: "Establishment");

            migrationBuilder.DropForeignKey(
                name: "FK_OpeningHours_EstablishmentInformation_EstablishmentInformat~",
                table: "OpeningHours");

            migrationBuilder.DropTable(
                name: "EstablishmentInformation");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_EstablishmentInformationId",
                table: "Establishment");

            migrationBuilder.DropColumn(
                name: "EstablishmentInformationId",
                table: "Establishment");

            migrationBuilder.RenameColumn(
                name: "EstablishmentInformationId",
                table: "OpeningHours",
                newName: "EstablishmentId");

            migrationBuilder.RenameIndex(
                name: "IX_OpeningHours_EstablishmentInformationId",
                table: "OpeningHours",
                newName: "IX_OpeningHours_EstablishmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Establishment",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Establishment_LocationId",
                table: "Establishment",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_Location_LocationId",
                table: "Establishment",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OpeningHours_Establishment_EstablishmentId",
                table: "OpeningHours",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id");
        }
    }
}
