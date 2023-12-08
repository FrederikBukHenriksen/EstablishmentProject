using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class _10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimestampStart",
                table: "Sale",
                newName: "TimestampArrival");

            migrationBuilder.RenameColumn(
                name: "TimestampEnd",
                table: "Sale",
                newName: "TimestampPayment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimestampPayment",
                table: "Sale",
                newName: "TimestampEnd");

            migrationBuilder.RenameColumn(
                name: "TimestampArrival",
                table: "Sale",
                newName: "TimestampStart");
        }
    }
}
