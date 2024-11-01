using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace National_Train_Reservation.Migrations
{
    /// <inheritdoc />
    public partial class train9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "AvailableTickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "carNumber",
                table: "AvailableTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "seatNumber",
                table: "AvailableTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "AvailableTickets");

            migrationBuilder.DropColumn(
                name: "carNumber",
                table: "AvailableTickets");

            migrationBuilder.DropColumn(
                name: "seatNumber",
                table: "AvailableTickets");
        }
    }
}
