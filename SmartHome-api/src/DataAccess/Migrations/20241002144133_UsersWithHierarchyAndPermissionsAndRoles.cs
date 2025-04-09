using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class UsersWithHierarchyAndPermissionsAndRoles : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CompanyOwners");

        migrationBuilder.CreateTable(
            name: "Permissions",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Permissions", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Roles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "RolePermissions",
            columns: table => new
            {
                PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_RolePermissions", x => new { x.PermissionId, x.RoleId });
                table.ForeignKey(
                    name: "FK_RolePermissions_Permissions_PermissionId",
                    column: x => x.PermissionId,
                    principalTable: "Permissions",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_RolePermissions_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                UserType = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                AssociatedCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AccountState = table.Column<bool>(type: "bit", nullable: true),
                ProfilePicture = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
                table.UniqueConstraint("AK_Users_Email", x => x.Email);
                table.ForeignKey(
                    name: "FK_Users_Companies_AssociatedCompanyId",
                    column: x => x.AssociatedCompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Users_Roles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.InsertData(
            table: "Permissions",
            columns: ["Id", "Name"],
            values: new object[,]
            {
                { new Guid("6321a816-3080-1002-aab7-5032779c3714"), "create-administrator" },
                { new Guid("6321a816-3080-1003-aab7-5032779c3714"), "create-companyowner" },
                { new Guid("6321a816-3080-1004-aab7-5032779c3714"), "list-users" },
                { new Guid("6321a816-3080-1005-aab7-5032779c3714"), "list-companies" }
            });

        migrationBuilder.InsertData(
            table: "Roles",
            columns: ["Id", "RoleName"],
            values: [new Guid("6321a816-3080-1001-aab7-5032779c3714"), "administrator"]);

        migrationBuilder.InsertData(
            table: "RolePermissions",
            columns: ["PermissionId", "RoleId"],
            values: new object[,]
            {
                { new Guid("6321a816-3080-1002-aab7-5032779c3714"), new Guid("6321a816-3080-1001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-1003-aab7-5032779c3714"), new Guid("6321a816-3080-1001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-1004-aab7-5032779c3714"), new Guid("6321a816-3080-1001-aab7-5032779c3714") },
                { new Guid("6321a816-3080-1005-aab7-5032779c3714"), new Guid("6321a816-3080-1001-aab7-5032779c3714") }
            });

        migrationBuilder.CreateIndex(
            name: "IX_RolePermissions_RoleId",
            table: "RolePermissions",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_AssociatedCompanyId",
            table: "Users",
            column: "AssociatedCompanyId");

        migrationBuilder.CreateIndex(
            name: "IX_Users_RoleId",
            table: "Users",
            column: "RoleId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "RolePermissions");

        migrationBuilder.DropTable(
            name: "Users");

        migrationBuilder.DropTable(
            name: "Permissions");

        migrationBuilder.DropTable(
            name: "Roles");

        migrationBuilder.CreateTable(
            name: "CompanyOwners",
            columns: table => new
            {
                Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                AssociatedCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                AccountState = table.Column<bool>(type: "bit", nullable: false),
                DateTimeCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Surname = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

        migrationBuilder.CreateIndex(
            name: "IX_CompanyOwners_AssociatedCompanyId",
            table: "CompanyOwners",
            column: "AssociatedCompanyId");
    }
}

[ExcludeFromCodeCoverage]
public partial class UsersWithHierarchyAndPermissionsAndRoles : Migration
{
}
