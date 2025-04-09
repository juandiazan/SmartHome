using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class NotificationAddAndHomeDeviceModification : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "HomeId",
            table: "HomeDevices",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TriggeringDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                TriggeringEvent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                WasRead = table.Column<bool>(type: "bit", nullable: false),
                DateTimeOfEvent = table.Column<DateTime>(type: "datetime2", nullable: false),
                UserItIsAddressedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
                table.ForeignKey(
                    name: "FK_Notifications_HomeDevices_TriggeringDeviceId",
                    column: x => x.TriggeringDeviceId,
                    principalTable: "HomeDevices",
                    principalColumn: "HardwareId",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Notifications_Homes_HomeId",
                    column: x => x.HomeId,
                    principalTable: "Homes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_HomeId",
            table: "Notifications",
            column: "HomeId");

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_TriggeringDeviceId",
            table: "Notifications",
            column: "TriggeringDeviceId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Notifications");

        migrationBuilder.DropColumn(
            name: "HomeId",
            table: "HomeDevices");
    }
}

[ExcludeFromCodeCoverage]
public partial class NotificationAddAndHomeDeviceModification : Migration
{
}
