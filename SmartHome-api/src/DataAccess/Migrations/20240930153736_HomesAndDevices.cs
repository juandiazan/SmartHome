using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class HomesAndDevices : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "CompanyOwners",
            columns: table => new
            {
                Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DateTimeCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                AssociatedCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AccountState = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CompanyOwners", x => x.Email);
                table.ForeignKey(
                    name: "FK_CompanyOwners_Companies_AssociatedCompanyId",
                    column: x => x.AssociatedCompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "Devices",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DeviceModel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Photos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                DeviceType = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Devices", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "HomeDevices",
            columns: table => new
            {
                HardwareId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ConnectionState = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_HomeDevices", x => x.HardwareId);
            });

        migrationBuilder.CreateTable(
            name: "Homes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                OwnerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address_MainStreet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address_DoorNumber = table.Column<int>(type: "int", nullable: false),
                Location_Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Location_Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MaxAmountOfMembers = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Homes", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CompanyOwners_AssociatedCompanyId",
            table: "CompanyOwners",
            column: "AssociatedCompanyId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CompanyOwners");

        migrationBuilder.DropTable(
            name: "Devices");

        migrationBuilder.DropTable(
            name: "HomeDevices");

        migrationBuilder.DropTable(
            name: "Homes");
    }
}

[ExcludeFromCodeCoverage]
public partial class HomesAndDevices : Migration
{
}
