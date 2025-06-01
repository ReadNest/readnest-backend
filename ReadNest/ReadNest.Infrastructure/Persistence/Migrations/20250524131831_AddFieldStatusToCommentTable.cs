using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldStatusToCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("55de61d6-6c12-4ec6-9600-e7b1df3a8f91"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("b1b59770-a2de-4127-8c20-b1e1e20a1b82"));

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("62df167f-32d9-49b8-9a69-7a5eec389014"), new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5073), false, "User", new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5074) },
                    { new Guid("cf56be60-21eb-4919-bc80-1f64694c6652"), new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5067), false, "Admin", new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5069) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("62df167f-32d9-49b8-9a69-7a5eec389014"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cf56be60-21eb-4919-bc80-1f64694c6652"));

            migrationBuilder.DropColumn(
                name: "status",
                table: "comments");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("55de61d6-6c12-4ec6-9600-e7b1df3a8f91"), new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5336), false, "User", new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5337) },
                    { new Guid("b1b59770-a2de-4127-8c20-b1e1e20a1b82"), new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5331), false, "Admin", new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5333) }
                });
        }
    }
}
