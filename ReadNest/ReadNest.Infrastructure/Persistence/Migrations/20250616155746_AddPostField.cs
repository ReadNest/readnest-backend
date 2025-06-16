using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPostField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("0b567d16-08c2-49bd-a84a-064116a86e91"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("c595eb59-7e85-4fa9-b189-8703272c9b95"));

            migrationBuilder.AddColumn<string>(
                name: "TitleNormalized",
                table: "posts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("1cf738c5-92af-4f58-8c3e-c1f90b138322"), new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(122), false, "Admin", new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(124) },
                    { new Guid("30c172a5-6b84-4d11-8253-7cc0994c8a68"), new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(127), false, "User", new DateTime(2025, 6, 16, 15, 57, 44, 570, DateTimeKind.Utc).AddTicks(127) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("1cf738c5-92af-4f58-8c3e-c1f90b138322"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("30c172a5-6b84-4d11-8253-7cc0994c8a68"));

            migrationBuilder.DropColumn(
                name: "TitleNormalized",
                table: "posts");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0b567d16-08c2-49bd-a84a-064116a86e91"), new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6498), false, "User", new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6498) },
                    { new Guid("c595eb59-7e85-4fa9-b189-8703272c9b95"), new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6493), false, "Admin", new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6496) }
                });
        }
    }
}
