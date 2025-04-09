using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeDeviceToRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomItIsInId",
                table: "HomeDevices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_RoomItIsInId",
                table: "HomeDevices",
                column: "RoomItIsInId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeDevices_Rooms_RoomItIsInId",
                table: "HomeDevices",
                column: "RoomItIsInId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeDevices_Rooms_RoomItIsInId",
                table: "HomeDevices");

            migrationBuilder.DropIndex(
                name: "IX_HomeDevices_RoomItIsInId",
                table: "HomeDevices");

            migrationBuilder.DropColumn(
                name: "RoomItIsInId",
                table: "HomeDevices");
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class AddHomeDeviceToRoom : Migration
    {
    }
}
