using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace National_Train_Reservation.Migrations
{
    /// <inheritdoc />
    public partial class train4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imagepath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagepath",
                table: "Users");
        }
    }
}
