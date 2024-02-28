using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class lolcat2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConfigurationId",
                table: "Establishment",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                    TableDataRetrivalIntegrationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SaleDataRetrivalIntegrationId = table.Column<Guid>(type: "uuid", nullable: true)
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
                name: "IX_Establishment_ConfigurationId",
                table: "Establishment",
                column: "ConfigurationId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentSettings_ConfigurationId",
                table: "Establishment",
                column: "ConfigurationId",
                principalTable: "EstablishmentSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentSettings_ConfigurationId",
                table: "Establishment");

            migrationBuilder.DropTable(
                name: "EstablishmentSettings");

            migrationBuilder.DropTable(
                name: "retrivedEntitiesJoiningTable");

            migrationBuilder.DropTable(
                name: "DataRetrivalIntegration");

            migrationBuilder.DropIndex(
                name: "IX_Establishment_ConfigurationId",
                table: "Establishment");

            migrationBuilder.DropColumn(
                name: "ConfigurationId",
                table: "Establishment");
        }
    }
}
