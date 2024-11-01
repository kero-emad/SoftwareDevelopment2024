using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace National_Train_Reservation.Migrations
{
    /// <inheritdoc />
    public partial class train2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "Journey_Price",
                table: "Trains",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Journey_Price",
                table: "Trains");

            migrationBuilder.AddColumn<int>(
                name: "Profile",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
