using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("1cf738c5-92af-4f58-8c3e-c1f90b138322"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("30c172a5-6b84-4d11-8253-7cc0994c8a68"));

            migrationBuilder.AddColumn<bool>(
                name: "is_read",
                table: "chat_messages",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "sent_at",
                table: "chat_messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("39c15c9c-7566-462f-a993-0699530f1cb0"), new DateTime(2025, 6, 19, 7, 16, 8, 446, DateTimeKind.Utc).AddTicks(7690), false, "Admin", new DateTime(2025, 6, 19, 7, 16, 8, 446, DateTimeKind.Utc).AddTicks(7692) },
                    { new Guid("e65180fb-c725-4d06-b0c0-cdae5f427bf9"), new DateTime(2025, 6, 19, 7, 16, 8, 446, DateTimeKind.Utc).AddTicks(7694), false, "User", new DateTime(2025, 6, 19, 7, 16, 8, 446, DateTimeKind.Utc).AddTicks(7695) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("39c15c9c-7566-462f-a993-0699530f1cb0"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("e65180fb-c725-4d06-b0c0-cdae5f427bf9"));

            migrationBuilder.DropColumn(
                name: "is_read",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "sent_at",
                table: "chat_messages");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1cf738c5-92af-4f58-8c3e-c1f90b138322"), new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(122), false, "Admin", new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(124) },
                    { new Guid("30c172a5-6b84-4d11-8253-7cc0994c8a68"), new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(127), false, "User", new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(127) }
                });
        }
    }
}
