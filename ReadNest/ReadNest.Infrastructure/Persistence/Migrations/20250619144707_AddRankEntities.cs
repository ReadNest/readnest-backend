using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRankEntities : Migration
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

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_events", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "event_rewards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    condition_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    threshold = table.Column<int>(type: "integer", nullable: false),
                    badge_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_event_rewards", x => x.id);
                    table.ForeignKey(
                        name: "fk_event_rewards_badge_id",
                        column: x => x.badge_id,
                        principalTable: "badges",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_rewards_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leaderboards",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPosts = table.Column<int>(type: "integer", nullable: false),
                    TotalLikes = table.Column<int>(type: "integer", nullable: false),
                    TotalViews = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    rank = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaderboards", x => x.id);
                    table.ForeignKey(
                        name: "fk_leaderboards_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_leaderboards_user_id",
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
                    { new Guid("87859145-ac0b-4b20-8983-7870d80e1abd"), new DateTime(2025, 6, 19, 14, 47, 5, 56, DateTimeKind.Utc).AddTicks(1576), false, "User", new DateTime(2025, 6, 19, 14, 47, 5, 56, DateTimeKind.Utc).AddTicks(1577) },
                    { new Guid("ab6374cb-29f7-468f-8977-7fabcd66f797"), new DateTime(2025, 6, 19, 14, 47, 5, 56, DateTimeKind.Utc).AddTicks(1572), false, "Admin", new DateTime(2025, 6, 19, 14, 47, 5, 56, DateTimeKind.Utc).AddTicks(1573) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_event_rewards_badge_id",
                table: "event_rewards",
                column: "badge_id");

            migrationBuilder.CreateIndex(
                name: "IX_event_rewards_event_id",
                table: "event_rewards",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_leaderboards_event_id",
                table: "leaderboards",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "IX_leaderboards_user_id",
                table: "leaderboards",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_rewards");

            migrationBuilder.DropTable(
                name: "leaderboards");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("87859145-ac0b-4b20-8983-7870d80e1abd"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("ab6374cb-29f7-468f-8977-7fabcd66f797"));

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
