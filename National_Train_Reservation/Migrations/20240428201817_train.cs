using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace National_Train_Reservation.Migrations
{
    /// <inheritdoc />
    public partial class train : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    Journey_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainNumber = table.Column<int>(type: "int", nullable: false),
                    Number_of_available_tickets = table.Column<int>(type: "int", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Launching_Station = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date_Pickup = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time_Pickup = table.Column<TimeSpan>(type: "time", nullable: false),
                    Number_of_stoppages = table.Column<int>(type: "int", nullable: false),
                    Arrival_Time = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.Journey_ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    National_Pass_Number = table.Column<long>(type: "bigint", nullable: false),
                    User_Fname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_Lname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Card_Number = table.Column<int>(type: "int", nullable: false),
                    Expiration_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CVV = table.Column<int>(type: "int", nullable: false),
                    User_Phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payment_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Suggestions_Complaints",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message_Type = table.Column<int>(type: "int", nullable: true),
                    First_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestions_Complaints", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Suggestions_Complaints_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time_Pickup = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Pickup_Station = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Seat_Number = table.Column<int>(type: "int", nullable: false),
                    Ticket_Money = table.Column<double>(type: "float", nullable: false),
                    Class = table.Column<int>(type: "int", nullable: false),
                    Profile = table.Column<int>(type: "int", nullable: false),
                    Train_Car_Number = table.Column<int>(type: "int", nullable: false),
                    User_ID = table.Column<int>(type: "int", nullable: false),
                    Train_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tickets_Trains_Train_ID",
                        column: x => x.Train_ID,
                        principalTable: "Trains",
                        principalColumn: "Journey_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_User_ID",
                        column: x => x.User_ID,
                        principalTable: "Users",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_User_ID",
                table: "Payment",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestions_Complaints_UserID",
                table: "Suggestions_Complaints",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Train_ID",
                table: "Tickets",
                column: "Train_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_User_ID",
                table: "Tickets",
                column: "User_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Suggestions_Complaints");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Trains");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
