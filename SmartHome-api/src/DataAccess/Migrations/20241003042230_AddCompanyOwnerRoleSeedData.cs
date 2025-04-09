using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class AddCompanyOwnerRoleSeedData : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users");

        migrationBuilder.InsertData(
            table: "Permissions",
            columns: ["Id", "Name"],
            values: new object[,]
            {
                { new Guid("6321a816-3080-2002-aab7-5032779c3714"), "create-company" },
                { new Guid("6321a816-3080-2003-aab7-5032779c3714"), "create-camera" },
                { new Guid("6321a816-3080-2004-aab7-5032779c3714"), "create-sensor" }
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: ["Id", "RoleName"],
            values: [new Guid("6321a816-3080-2001-aab7-5032779c3714"), "company-owner"]);

        migrationBuilder.InsertData(
            table: "RolePermissions",
            columns: ["PermissionId", "RoleId"],
            values: new object[,]
            {
                { new Guid("6321a816-3080-2002-aab7-5032779c3714"), new Guid("6321a816-3080-2001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-2003-aab7-5032779c3714"), new Guid("6321a816-3080-2001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-2004-aab7-5032779c3714"), new Guid("6321a816-3080-2001-aab7-5032779c3714") }
            });

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users",
            column: "RoleId",
            principalTable: "Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users");

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-2002-aab7-5032779c3714"), new Guid("6321a816-3080-2001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-2003-aab7-5032779c3714"), new Guid("6321a816-3080-2001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-2004-aab7-5032779c3714"), new Guid("6321a816-3080-2001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-2002-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-2003-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-2004-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-2001-aab7-5032779c3714"));

        migrationBuilder.AddForeignKey(
            name: "FK_Users_Roles_RoleId",
            table: "Users",
            column: "RoleId",
            principalTable: "Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}

[ExcludeFromCodeCoverage]
public partial class AddCompanyOwnerRoleSeedData : Migration
{
}
