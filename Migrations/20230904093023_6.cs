using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId1",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_EstablishmentId1",
                table: "Location",
                column: "EstablishmentId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Establishment_EstablishmentId1",
                table: "Location",
                column: "EstablishmentId1",
                principalTable: "Establishment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Establishment_EstablishmentId1",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_EstablishmentId1",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "EstablishmentId1",
                table: "Location");
        }
    }
}
