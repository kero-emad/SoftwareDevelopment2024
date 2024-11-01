using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace National_Train_Reservation.Migrations
{
    /// <inheritdoc />
    public partial class train3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Trains_Train_ID",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Train_ID",
                table: "Tickets",
                newName: "Journey_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_Train_ID",
                table: "Tickets",
                newName: "IX_Tickets_Journey_ID");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Time_Pickup",
                table: "Tickets",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Class",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date_pickup",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Trains_Journey_ID",
                table: "Tickets",
                column: "Journey_ID",
                principalTable: "Trains",
                principalColumn: "Journey_ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Trains_Journey_ID",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Date_pickup",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "Journey_ID",
                table: "Tickets",
                newName: "Train_ID");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_Journey_ID",
                table: "Tickets",
                newName: "IX_Tickets_Train_ID");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time_Pickup",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<int>(
                name: "Class",
                table: "Tickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Trains_Train_ID",
                table: "Tickets",
                column: "Train_ID",
                principalTable: "Trains",
                principalColumn: "Journey_ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
