using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExaminationSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class FinalAuthSetup_Fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), "D1111111-1111-1111-1111-111111111111", "Admin", "ADMIN" },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), "E2222222-2222-2222-2222-222222222222", "Student", "STUDENT" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DeletedAt", "Email", "EmailConfirmed", "FullName", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "Status", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("c3333333-3333-3333-3333-333333333333"), 0, "G4444444-4444-4444-4444-444444444444", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "ahmedali660@gmail.com", true, "Ahmed Ali", false, false, null, "AHMEDALI660@GMAIL.COM", "AHMEDALI660", "AQAAAAIAAYagAAAAEE6mfs5nlUwXENNWDXGrILwjOLAVq/UkeXlSCmM8qnKQVk6qO9H3AX8m7TgsHIa7Ag==", null, false, 1, "F3333333-3333-3333-3333-333333333333", 1, false, null, "ahmedali660" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("c3333333-3333-3333-3333-333333333333") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b2222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("a1111111-1111-1111-1111-111111111111"), new Guid("c3333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a1111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c3333333-3333-3333-3333-333333333333"));
        }
    }
}
