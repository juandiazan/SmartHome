using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HomeDeviceAliasChangePermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("6321a816-3080-4005-aab7-5032779c3714"), "change-alias-of-specific-device" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[] { new Guid("6321a816-3080-4005-aab7-5032779c3714"), new Guid("6321a816-3080-4001-aab7-5032779c3714") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-4005-aab7-5032779c3714"), new Guid("6321a816-3080-4001-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6321a816-3080-4005-aab7-5032779c3714"));
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class HomeDeviceAliasChangePermission : Migration
    {
    }
}
