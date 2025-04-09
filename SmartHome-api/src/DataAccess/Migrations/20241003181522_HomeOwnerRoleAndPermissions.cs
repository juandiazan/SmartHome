using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class HomeOwnerRoleAndPermissions : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.InsertData(
            table: "Permissions",
            columns: ["Id", "Name"],
            values: new object[,]
            {
                { new Guid("6321a816-3080-3002-aab7-5032779c3714"), "create-home" },
                { new Guid("6321a816-3080-3003-aab7-5032779c3714"), "add-member-to-home" },
                { new Guid("6321a816-3080-3004-aab7-5032779c3714"), "add-device-to-home" },
                { new Guid("6321a816-3080-3005-aab7-5032779c3714"), "list-members-of-home" },
                { new Guid("6321a816-3080-3006-aab7-5032779c3714"), "list-devices-of-home" },
                { new Guid("6321a816-3080-3007-aab7-5032779c3714"), "add-permissions-to-member" }
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: ["Id", "RoleName"],
            values: [new Guid("6321a816-3080-3001-aab7-5032779c3714"), "home-owner"]);

        migrationBuilder.InsertData(
            table: "RolePermissions",
            columns: ["PermissionId", "RoleId"],
            values: new object[,]
            {
                { new Guid("6321a816-3080-3002-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-3003-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-3004-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-3005-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-3006-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-3007-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714") }
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-3002-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-3003-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-3004-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-3005-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-3006-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "RolePermissions",
            keyColumns: ["PermissionId", "RoleId"],
            keyValues: [new Guid("6321a816-3080-3007-aab7-5032779c3714"), new Guid("6321a816-3080-3001-aab7-5032779c3714")]);

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3002-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3003-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3004-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3005-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3006-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Permissions",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3007-aab7-5032779c3714"));

        migrationBuilder.DeleteData(
            table: "Roles",
            keyColumn: "Id",
            keyValue: new Guid("6321a816-3080-3001-aab7-5032779c3714"));
    }
}

[ExcludeFromCodeCoverage]
public partial class HomeOwnerRoleAndPermissions : Migration
{
}
