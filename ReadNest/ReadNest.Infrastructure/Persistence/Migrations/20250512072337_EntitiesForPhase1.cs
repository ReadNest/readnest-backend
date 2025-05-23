using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EntitiesForPhase1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("762ff468-377e-4c59-8f2d-552ce8fbc8b0"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("b9f3bef5-2c04-44ed-a638-e52e9690a460"));

            _ = migrationBuilder.CreateTable(
                name: "affiliate_links",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    partner_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_affiliate_links", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_affiliate_links_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_categories", x => x.id);
                });

            _ = migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_comments", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_comments_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "fk_comments_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "book_categories",
                columns: table => new
                {
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_book_categories", x => new { x.book_id, x.category_id });
                    _ = table.ForeignKey(
                        name: "fk_book_categories_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "fk_book_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "comment_likes",
                columns: table => new
                {
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_comment_likes", x => new { x.comment_id, x.user_id });
                    _ = table.ForeignKey(
                        name: "fk_comment_likes_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "fk_comment_likes_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2f6ffbe7-246f-456b-9b5b-77f71be1ad78"), new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8943), false, "User", new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8944) },
                    { new Guid("845af63e-f0c8-4fb1-91b0-e7ea1cec49f1"), new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8938), false, "Admin", new DateTime(2025, 5, 12, 7, 23, 36, 926, DateTimeKind.Utc).AddTicks(8940) }
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_affiliate_links_book_id",
                table: "affiliate_links",
                column: "book_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_book_categories_category_id",
                table: "book_categories",
                column: "category_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_comment_likes_user_id",
                table: "comment_likes",
                column: "user_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_comments_book_id",
                table: "comments",
                column: "book_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_comments_user_id",
                table: "comments",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "affiliate_links");

            _ = migrationBuilder.DropTable(
                name: "book_categories");

            _ = migrationBuilder.DropTable(
                name: "comment_likes");

            _ = migrationBuilder.DropTable(
                name: "categories");

            _ = migrationBuilder.DropTable(
                name: "comments");

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("2f6ffbe7-246f-456b-9b5b-77f71be1ad78"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("845af63e-f0c8-4fb1-91b0-e7ea1cec49f1"));

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("762ff468-377e-4c59-8f2d-552ce8fbc8b0"), new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7050), false, "Admin", new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7054) },
                    { new Guid("b9f3bef5-2c04-44ed-a638-e52e9690a460"), new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7057), false, "User", new DateTime(2025, 5, 12, 6, 46, 13, 79, DateTimeKind.Utc).AddTicks(7057) }
                });
        }
    }
}
