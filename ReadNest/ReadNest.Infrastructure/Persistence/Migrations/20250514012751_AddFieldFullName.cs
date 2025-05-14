using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("2f6ffbe7-246f-456b-9b5b-77f71be1ad78"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("845af63e-f0c8-4fb1-91b0-e7ea1cec49f1"));

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0c2d0b97-0520-4af3-ab0a-211295f8192a"), new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6094), false, "User", new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6095) },
                    { new Guid("9d5ac316-0291-4983-a765-4ef7d9e64bf6"), new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6087), false, "Admin", new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6093) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("0c2d0b97-0520-4af3-ab0a-211295f8192a"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("9d5ac316-0291-4983-a765-4ef7d9e64bf6"));

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "users");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2f6ffbe7-246f-456b-9b5b-77f71be1ad78"), new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8943), false, "User", new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8944) },
                    { new Guid("845af63e-f0c8-4fb1-91b0-e7ea1cec49f1"), new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8938), false, "Admin", new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8940) }
                });
        }
    }
}
