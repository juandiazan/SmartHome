using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MembersFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssociatedHomeOwnerId",
                table: "Members",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HomeOwnerId",
                table: "Homes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Members_AssociatedHomeOwnerId",
                table: "Members",
                column: "AssociatedHomeOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Homes_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes",
                column: "HomeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Users_AssociatedHomeOwnerId",
                table: "Members",
                column: "AssociatedHomeOwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Homes_Users_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Users_AssociatedHomeOwnerId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_AssociatedHomeOwnerId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Homes_HomeOwnerId",
                table: "Homes");

            migrationBuilder.DropColumn(
                name: "AssociatedHomeOwnerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "HomeOwnerId",
                table: "Homes");
        }
    }
    [ExcludeFromCodeCoverage]
    public partial class MembersFix : Migration
    {
    }
}
