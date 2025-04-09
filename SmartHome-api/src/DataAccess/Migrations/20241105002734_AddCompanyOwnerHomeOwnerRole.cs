using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyOwnerHomeOwnerRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { new Guid("6321a816-3080-5002-aab7-5032779c3714"), "company-owner-home-owner" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("6321a816-3080-2002-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-2003-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-2004-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-3002-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-3003-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-3004-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-3005-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-3006-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-3007-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-2002-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-2003-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-2004-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-3002-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-3003-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-3004-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-3005-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-3006-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-3007-aab7-5032779c3714"), new Guid("6321a816-3080-5002-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6321a816-3080-5002-aab7-5032779c3714"));
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class AddCompanyOwnerHomeOwnerRole : Migration
    {
    }
}
