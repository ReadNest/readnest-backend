using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddEntitiesBookAndFavBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6d4fd7dd-14e3-41b8-b0a7-b93c5ad7ccdd"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("d7635345-a7d4-4403-b2e0-3ebab54322eb"));

            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    author = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    image_url = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    average_rating = table.Column<double>(type: "double precision", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "favorite_books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorite_books", x => x.id);
                    table.ForeignKey(
                        name: "FK_favorite_books_books_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorite_books_users_user_id",
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
                    { new Guid("762ff468-377e-4c59-8f2d-552ce8fbc8b0"), new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7050), false, "Admin", new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7054) },
                    { new Guid("b9f3bef5-2c04-44ed-a638-e52e9690a460"), new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7057), false, "User", new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7057) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_favorite_books_book_id",
                table: "favorite_books",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorite_books_user_id",
                table: "favorite_books",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorite_books");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("762ff468-377e-4c59-8f2d-552ce8fbc8b0"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("b9f3bef5-2c04-44ed-a638-e52e9690a460"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("6d4fd7dd-14e3-41b8-b0a7-b93c5ad7ccdd"), new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2109), false, "Admin", new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2113) },
                    { new Guid("d7635345-a7d4-4403-b2e0-3ebab54322eb"), new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2115), false, "User", new DateTime(2025, 5, 1, 2, 42, 49, 489, DateTimeKind.Utc).AddTicks(2115) }
                });
        }
    }
}
