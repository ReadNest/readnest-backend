using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableUserSearchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("07d087ac-6e41-4db5-8815-867c9dd8b2e7"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("adc4b013-1a38-4350-91b3-cc394ce17f80"));

            migrationBuilder.CreateTable(
                name: "user_search_histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    keyword = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_search_histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_search_histories_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("65208406-ad8d-432f-a4cc-28ea95ab2e26"), new DateTime(2025, 7, 24, 2, 12, 47, 336, DateTimeKind.Utc).AddTicks(800), false, "Admin", new DateTime(2025, 7, 24, 2, 12, 47, 336, DateTimeKind.Utc).AddTicks(803) },
                    { new Guid("cb19482f-5d37-4da4-b352-d645bbd0f850"), new DateTime(2025, 7, 24, 2, 12, 47, 336, DateTimeKind.Utc).AddTicks(806), false, "User", new DateTime(2025, 7, 24, 2, 12, 47, 336, DateTimeKind.Utc).AddTicks(806) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_search_histories_user_id",
                table: "user_search_histories",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_search_histories");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("65208406-ad8d-432f-a4cc-28ea95ab2e26"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cb19482f-5d37-4da4-b352-d645bbd0f850"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("07d087ac-6e41-4db5-8815-867c9dd8b2e7"), new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(56), false, "Admin", new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(58) },
                    { new Guid("adc4b013-1a38-4350-91b3-cc394ce17f80"), new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(62), false, "User", new DateTime(2025, 7, 10, 1, 37, 52, 660, DateTimeKind.Utc).AddTicks(62) }
                });
        }
    }
}
