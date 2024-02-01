using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class lolcat10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentSettings_ConfigurationId",
                table: "Establishment");

            migrationBuilder.RenameColumn(
                name: "ConfigurationId",
                table: "Establishment",
                newName: "SettingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Establishment_ConfigurationId",
                table: "Establishment",
                newName: "IX_Establishment_SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentSettings_SettingsId",
                table: "Establishment",
                column: "SettingsId",
                principalTable: "EstablishmentSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_EstablishmentSettings_SettingsId",
                table: "Establishment");

            migrationBuilder.RenameColumn(
                name: "SettingsId",
                table: "Establishment",
                newName: "ConfigurationId");

            migrationBuilder.RenameIndex(
                name: "IX_Establishment_SettingsId",
                table: "Establishment",
                newName: "IX_Establishment_ConfigurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_EstablishmentSettings_ConfigurationId",
                table: "Establishment",
                column: "ConfigurationId",
                principalTable: "EstablishmentSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
