using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Location",
                newName: "Coordinates_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Location",
                newName: "Coordinates_Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Coordinates_Longitude",
                table: "Location",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Coordinates_Latitude",
                table: "Location",
                newName: "Latitude");
        }
    }
}
