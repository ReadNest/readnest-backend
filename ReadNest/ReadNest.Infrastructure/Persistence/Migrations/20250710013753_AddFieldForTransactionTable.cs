using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldForTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("253e057e-32c3-4bbf-bc50-2ffcffa1c2d0"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6cebc411-0dc2-4076-b923-2c19b9f2af1c"));

            migrationBuilder.AddColumn<long>(
                name: "order_code",
                table: "transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("07d087ac-6e41-4db5-8815-867c9dd8b2e7"), new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(56), false, "Admin", new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(58) },
                    { new Guid("adc4b013-1a38-4350-91b3-cc394ce17f80"), new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(62), false, "User", new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(62) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("07d087ac-6e41-4db5-8815-867c9dd8b2e7"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("adc4b013-1a38-4350-91b3-cc394ce17f80"));

            migrationBuilder.DropColumn(
                name: "order_code",
                table: "transactions");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("253e057e-32c3-4bbf-bc50-2ffcffa1c2d0"), new DateTime(2025, 7, 8, 14, 18, 21, 266, DateTimeKind.Utc).AddTicks(2037), false, "User", new DateTime(2025, 7, 8, 14, 18, 21, 266, DateTimeKind.Utc).AddTicks(2037) },
                    { new Guid("6cebc411-0dc2-4076-b923-2c19b9f2af1c"), new DateTime(2025, 7, 8, 14, 18, 21, 266, DateTimeKind.Utc).AddTicks(2029), false, "Admin", new DateTime(2025, 7, 8, 14, 18, 21, 266, DateTimeKind.Utc).AddTicks(2034) }
                });
        }
    }
}
