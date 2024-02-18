using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class OKay2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentSettings_SettingsId",
                table: "Establishment");

            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_information_InformationId",
                table: "Establishment");

            migrationBuilder.DropTable(
                name: "EstablishmentSettings");

            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.DropTable(
                name: "retrivedEntitiesJoiningTable");

            migrationBuilder.DropTable(
                name: "DataRetrivalIntegration");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_SettingsId",
                table: "Establishment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_information",
                table: "information");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Establishment");

            migrationBuilder.RenameTable(
                name: "information",
                newName: "EstablishmentInformation");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "EstablishmentInformation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstablishmentInformation",
                table: "EstablishmentInformation",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_EstablishmentInformation_Location_LocationId",
                table: "EstablishmentInformation",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentInformation_InformationId",
                table: "Establishment");

            migrationBuilder.DropForeignKey(
                name: "FK_EstablishmentInformation_Location_LocationId",
                table: "EstablishmentInformation");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstablishmentInformation",
                table: "EstablishmentInformation");

            migrationBuilder.DropIndex(
                name: "IX_EstablishmentInformation_LocationId",
                table: "EstablishmentInformation");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "EstablishmentInformation");

            migrationBuilder.RenameTable(
                name: "EstablishmentInformation",
                newName: "information");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                table: "Establishment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_information",
                table: "information",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DataRetrivalIntegration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeOfService = table.Column<int>(type: "integer", nullable: false),
                    credentials = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataRetrivalIntegration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EstablishmentInformationId = table.Column<Guid>(type: "uuid", nullable: true),
                    close = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    dayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    open = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningHours_information_EstablishmentInformationId",
                        column: x => x.EstablishmentInformationId,
                        principalTable: "information",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "retrivedEntitiesJoiningTable",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    ForeignId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retrivedEntitiesJoiningTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemDataRetrivalIntegrationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaleDataRetrivalIntegrationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TableDataRetrivalIntegrationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstablishmentSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstablishmentSettings_DataRetrivalIntegration_ItemDataRetri~",
                        column: x => x.ItemDataRetrivalIntegrationId,
                        principalTable: "DataRetrivalIntegration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EstablishmentSettings_DataRetrivalIntegration_SaleDataRetri~",
                        column: x => x.SaleDataRetrivalIntegrationId,
                        principalTable: "DataRetrivalIntegration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EstablishmentSettings_DataRetrivalIntegration_TableDataRetr~",
                        column: x => x.TableDataRetrivalIntegrationId,
                        principalTable: "DataRetrivalIntegration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Establishment_SettingsId",
                table: "Establishment",
                column: "SettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentSettings_ItemDataRetrivalIntegrationId",
                table: "EstablishmentSettings",
                column: "ItemDataRetrivalIntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentSettings_SaleDataRetrivalIntegrationId",
                table: "EstablishmentSettings",
                column: "SaleDataRetrivalIntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_EstablishmentSettings_TableDataRetrivalIntegrationId",
                table: "EstablishmentSettings",
                column: "TableDataRetrivalIntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_EstablishmentInformationId",
                table: "OpeningHours",
                column: "EstablishmentInformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentSettings_SettingsId",
                table: "Establishment",
                column: "SettingsId",
                principalTable: "EstablishmentSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_information_InformationId",
                table: "Establishment",
                column: "InformationId",
                principalTable: "information",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
