using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSmartLampRedundantColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmartLampModel",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SmartLampName",
                table: "Devices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmartLampModel",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmartLampName",
                table: "Devices",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class RemoveSmartLampRedundantColumns : Migration
    {
    }
}
