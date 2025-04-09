using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AdminSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "RoleId", "Surname", "UserType" },
                values: new object[] { new Guid("00000001-6618-4bab-a6b6-9a32a11893f8"), "administrator@gmail.com", "User", "admin123!", new Guid("6321a816-3080-1001-aab7-5032779c3714"), "Admin", "administrator" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("00000001-6618-4bab-a6b6-9a32a11893f8"));
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class AdminSeedData : Migration
    {
    }
}
