using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations;

/// <inheritdoc />
public partial class CompanyAssociationToOwner : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Users_AssociatedCompanyId",
            table: "Users");

        migrationBuilder.CreateIndex(
            name: "IX_Users_AssociatedCompanyId",
            table: "Users",
            column: "AssociatedCompanyId",
            unique: true,
            filter: "[AssociatedCompanyId] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Users_AssociatedCompanyId",
            table: "Users");

        migrationBuilder.CreateIndex(
            name: "IX_Users_AssociatedCompanyId",
            table: "Users",
            column: "AssociatedCompanyId");
    }
}

[ExcludeFromCodeCoverage]
public partial class CompanyAssociationToOwner : Migration
{
}
