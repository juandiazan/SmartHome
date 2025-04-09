using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSmartLamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TypeOfDevice",
                table: "Devices",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(8)",
                oldMaxLength: 8);

            migrationBuilder.AddColumn<bool>(
                name: "IsTurnedOn",
                table: "Devices",
                type: "bit",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTurnedOn",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SmartLampModel",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "SmartLampName",
                table: "Devices");

            migrationBuilder.AlterColumn<string>(
                name: "TypeOfDevice",
                table: "Devices",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class AddSmartLamp : Migration
    {
    }
}
