using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Establishment_EstablishmentId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_Location_Establishment_EstablishmentId1",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_EstablishmentId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_EstablishmentId1",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "EstablishmentId1",
                table: "Location");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Establishment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_Establishment_Location_Id",
                table: "Establishment",
                column: "Id",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Establishment_Location_Id",
                table: "Establishment");

            migrationBuilder.AddColumn<int>(
                name: "EstablishmentId1",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Establishment",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_Location_EstablishmentId",
                table: "Location",
                column: "EstablishmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_EstablishmentId1",
                table: "Location",
                column: "EstablishmentId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Establishment_EstablishmentId",
                table: "Location",
                column: "EstablishmentId",
                principalTable: "Establishment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Establishment_EstablishmentId1",
                table: "Location",
                column: "EstablishmentId1",
                principalTable: "Establishment",
                principalColumn: "Id");
        }
    }
}
