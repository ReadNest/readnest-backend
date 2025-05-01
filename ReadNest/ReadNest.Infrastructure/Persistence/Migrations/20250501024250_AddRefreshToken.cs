using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6057fc76-926e-4926-bc8c-273fcd0b43cd"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6c9e5203-ede3-4437-b9a5-eb6d3f394a3a"));

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_tokens", x => x.Id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6d4fd7dd-14e3-41b8-b0a7-b93c5ad7ccdd"), new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2109), false, "Admin", new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2113) },
                    { new Guid("d7635345-a7d4-4403-b2e0-3ebab54322eb"), new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2115), false, "User", new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2115) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_tokens_UserId",
                table: "refresh_tokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6d4fd7dd-14e3-41b8-b0a7-b93c5ad7ccdd"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("d7635345-a7d4-4403-b2e0-3ebab54322eb"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6057fc76-926e-4926-bc8c-273fcd0b43cd"), new DateTime(2025, 4, 30, 16, 20, 34, 609, DateTimeKind.Utc).AddTicks(4526), false, "Admin", new DateTime(2025, 4, 30, 16, 20, 34, 609, DateTimeKind.Utc).AddTicks(4528) },
                    { new Guid("6c9e5203-ede3-4437-b9a5-eb6d3f394a3a"), new DateTime(2025, 4, 30, 16, 20, 34, 609, DateTimeKind.Utc).AddTicks(4530), false, "User", new DateTime(2025, 4, 30, 16, 20, 34, 609, DateTimeKind.Utc).AddTicks(4530) }
                });
        }
    }
}
