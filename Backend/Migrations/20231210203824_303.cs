using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _303 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "Establishment");

            migrationBuilder.CreateTable(
                name: "OpeningHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    dayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    open = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    close = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    EstablishmentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpeningHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpeningHours_Establishment_EstablishmentId",
                        column: x => x.EstablishmentId,
                        principalTable: "Establishment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpeningHours_EstablishmentId",
                table: "OpeningHours",
                column: "EstablishmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpeningHours");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "Establishment",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
