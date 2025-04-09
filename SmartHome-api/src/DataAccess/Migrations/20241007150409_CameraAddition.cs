using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CameraAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanBeUsedIndoors",
                table: "Devices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanBeUsedOutdoors",
                table: "Devices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasMovementDetectionSupport",
                table: "Devices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasPersonDetectionSupport",
                table: "Devices",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanBeUsedIndoors",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "CanBeUsedOutdoors",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "HasMovementDetectionSupport",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "HasPersonDetectionSupport",
                table: "Devices");
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class CameraAddition : Migration
    {
    }
}
