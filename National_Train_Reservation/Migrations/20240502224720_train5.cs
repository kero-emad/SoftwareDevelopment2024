using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace National_Train_Reservation.Migrations
{
    /// <inheritdoc />
    public partial class train5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Card_Owner_name",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Card_Owner_name",
                table: "Payment");
        }
    }
}
