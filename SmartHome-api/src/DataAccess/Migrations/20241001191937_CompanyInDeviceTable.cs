using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class CompanyInDeviceTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "CompanyId",
            table: "Devices",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_Devices_CompanyId",
            table: "Devices",
            column: "CompanyId");

        migrationBuilder.AddForeignKey(
            name: "FK_Devices_Companies_CompanyId",
            table: "Devices",
            column: "CompanyId",
            principalTable: "Companies",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Devices_Companies_CompanyId",
            table: "Devices");

        migrationBuilder.DropIndex(
            name: "IX_Devices_CompanyId",
            table: "Devices");

        migrationBuilder.DropColumn(
            name: "CompanyId",
            table: "Devices");
    }
}

[ExcludeFromCodeCoverage]
public partial class CompanyInDeviceTable : Migration
{
}
