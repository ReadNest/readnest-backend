using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPostEntity : Migration
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

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    views = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.id);
                    table.ForeignKey(
                        name: "fk_posts_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_posts_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "post_likes",
                columns: table => new
                {
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_likes", x => new { x.post_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_post_likes_comment_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_post_likes_user_id",
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
                    { new Guid("179c6cc0-165c-4e4d-8ef1-f6ff7792d076"), new DateTime(2025, 5, 28, 11, 15, 40, 640, DateTimeKind.Utc).AddTicks(3866), false, "Admin", new DateTime(2025, 5, 28, 11, 15, 40, 640, DateTimeKind.Utc).AddTicks(3868) },
                    { new Guid("6458791b-5bc8-4c0c-a770-d37f1e1ff74c"), new DateTime(2025, 5, 28, 11, 15, 40, 640, DateTimeKind.Utc).AddTicks(3870), false, "User", new DateTime(2025, 5, 28, 11, 15, 40, 640, DateTimeKind.Utc).AddTicks(3871) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_post_likes_user_id",
                table: "post_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_book_id",
                table: "posts",
                column: "book_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_user_id",
                table: "posts",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "post_likes");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("179c6cc0-165c-4e4d-8ef1-f6ff7792d076"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("6458791b-5bc8-4c0c-a770-d37f1e1ff74c"));

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
