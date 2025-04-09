using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomeDeviceFKFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HomeDevices_DeviceId",
                table: "HomeDevices",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeDevices_Devices_DeviceId",
                table: "HomeDevices",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeDevices_Devices_DeviceId",
                table: "HomeDevices");

            migrationBuilder.DropIndex(
                name: "IX_HomeDevices_DeviceId",
                table: "HomeDevices");
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class HomeDeviceFKFix : Migration
    {
    }
}
