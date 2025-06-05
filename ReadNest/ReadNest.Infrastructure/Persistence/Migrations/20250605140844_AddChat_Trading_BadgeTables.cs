using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddChat_Trading_BadgeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("50deb9c7-b5f2-41d5-a79a-cef55b29cfce"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("f2bf4ed5-9634-4c60-b62a-e4b5cf9c2df3"));

            migrationBuilder.CreateTable(
                name: "badges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_messages_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_messages_sender_id",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trading_posts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferedBookId = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    condition = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trading_posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_trading_posts_books_OfferedBookId",
                        column: x => x.OfferedBookId,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trading_posts_user_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_badges",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    badge_id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_badges", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_badges_badge_id",
                        column: x => x.badge_id,
                        principalTable: "badges",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_badges_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trading_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trading_post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    requester_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trading_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_trading_requests_requester_id",
                        column: x => x.requester_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trading_requests_trading_post_id",
                        column: x => x.trading_post_id,
                        principalTable: "trading_posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0b567d16-08c2-49bd-a84a-064116a86e91"), new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6498), false, "User", new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6498) },
                    { new Guid("c595eb59-7e85-4fa9-b189-8703272c9b95"), new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6493), false, "Admin", new DateTime(2025, 6, 5, 14, 8, 42, 664, DateTimeKind.Utc).AddTicks(6496) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_badges_code",
                table: "badges",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_receiver_id",
                table: "chat_messages",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "IX_chat_messages_sender_id",
                table: "chat_messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_trading_posts_OfferedBookId",
                table: "trading_posts",
                column: "OfferedBookId");

            migrationBuilder.CreateIndex(
                name: "IX_trading_posts_owner_id",
                table: "trading_posts",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_trading_requests_requester_id",
                table: "trading_requests",
                column: "requester_id");

            migrationBuilder.CreateIndex(
                name: "IX_trading_requests_trading_post_id",
                table: "trading_requests",
                column: "trading_post_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_badges_badge_id",
                table: "user_badges",
                column: "badge_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_badges_user_id",
                table: "user_badges",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.DropTable(
                name: "trading_requests");

            migrationBuilder.DropTable(
                name: "user_badges");

            migrationBuilder.DropTable(
                name: "trading_posts");

            migrationBuilder.DropTable(
                name: "badges");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("0b567d16-08c2-49bd-a84a-064116a86e91"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("c595eb59-7e85-4fa9-b189-8703272c9b95"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("50deb9c7-b5f2-41d5-a79a-cef55b29cfce"), new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5257), false, "User", new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5257) },
                    { new Guid("f2bf4ed5-9634-4c60-b62a-e4b5cf9c2df3"), new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5253), false, "Admin", new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5255) }
                });
        }
    }
}
