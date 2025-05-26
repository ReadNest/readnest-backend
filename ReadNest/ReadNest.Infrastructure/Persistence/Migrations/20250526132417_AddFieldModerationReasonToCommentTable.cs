using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldModerationReasonToCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("62df167f-32d9-49b8-9a69-7a5eec389014"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cf56be60-21eb-4919-bc80-1f64694c6652"));

            migrationBuilder.AddColumn<string>(
                name: "moderation_reason",
                table: "comments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("582d5dce-f681-4be0-beb7-20a549b780f0"), new DateTime(2025, 5, 26, 13, 24, 15, 940, DateTimeKind.Utc).AddTicks(4227), false, "Admin", new DateTime(2025, 5, 26, 13, 24, 15, 940, DateTimeKind.Utc).AddTicks(4235) },
                    { new Guid("6bd945b4-c12e-4034-85b1-d8a8ab572ade"), new DateTime(2025, 5, 26, 13, 24, 15, 940, DateTimeKind.Utc).AddTicks(4240), false, "User", new DateTime(2025, 5, 26, 13, 24, 15, 940, DateTimeKind.Utc).AddTicks(4241) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("582d5dce-f681-4be0-beb7-20a549b780f0"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6bd945b4-c12e-4034-85b1-d8a8ab572ade"));

            migrationBuilder.DropColumn(
                name: "moderation_reason",
                table: "comments");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("62df167f-32d9-49b8-9a69-7a5eec389014"), new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5073), false, "User", new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5074) },
                    { new Guid("cf56be60-21eb-4919-bc80-1f64694c6652"), new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5067), false, "Admin", new DateTime(2025, 5, 24, 13, 18, 29, 219, DateTimeKind.Utc).AddTicks(5069) }
                });
        }
    }
}
