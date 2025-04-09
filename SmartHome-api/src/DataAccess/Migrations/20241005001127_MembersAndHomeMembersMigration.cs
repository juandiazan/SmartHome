using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MembersAndHomeMembersMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HomeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Homes_HomeId",
                        column: x => x.HomeId,
                        principalTable: "Homes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MemberPermissions",
                columns: table => new
                {
                    MembersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberPermissions", x => new { x.MembersId, x.PermissionsId });
                    table.ForeignKey(
                        name: "FK_MemberPermissions_Members_MembersId",
                        column: x => x.MembersId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberPermissions_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("6321a816-3080-4002-aab7-5032779c3714"), "add-device-to-specific-home" },
                    { new Guid("6321a816-3080-4003-aab7-5032779c3714"), "list-devices-of-specific-home" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "RoleName" },
                values: new object[] { new Guid("6321a816-3080-4001-aab7-5032779c3714"), "home-specific-permissions" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("6321a816-3080-4002-aab7-5032779c3714"), new Guid("6321a816-3080-4001-aab7-5032779c3714") },
                    { new Guid("6321a816-3080-4003-aab7-5032779c3714"), new Guid("6321a816-3080-4001-aab7-5032779c3714") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberPermissions_PermissionsId",
                table: "MemberPermissions",
                column: "PermissionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_HomeId",
                table: "Members",
                column: "HomeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberPermissions");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-4002-aab7-5032779c3714"), new Guid("6321a816-3080-4001-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("6321a816-3080-4003-aab7-5032779c3714"), new Guid("6321a816-3080-4001-aab7-5032779c3714") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6321a816-3080-4002-aab7-5032779c3714"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: new Guid("6321a816-3080-4003-aab7-5032779c3714"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6321a816-3080-4001-aab7-5032779c3714"));
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class MembersAndHomeMembersMigration : Migration
    {
    }
}
