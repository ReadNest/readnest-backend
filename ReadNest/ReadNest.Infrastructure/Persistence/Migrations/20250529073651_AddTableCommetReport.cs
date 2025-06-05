using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCommetReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("4d2b901b-cc8b-467d-962c-574a5be0479b"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cf114091-2437-4c17-8148-ed251cf7e2f0"));

            _ = migrationBuilder.CreateTable(
                name: "comment_reports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reporter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_comment_reports", x => x.id);
                    _ = table.ForeignKey(
                        name: "fk_comment_reports_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "fk_comment_reports_reporter_id",
                        column: x => x.reporter_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("50deb9c7-b5f2-41d5-a79a-cef55b29cfce"), new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5257), false, "User", new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5257) },
                    { new Guid("f2bf4ed5-9634-4c60-b62a-e4b5cf9c2df3"), new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5253), false, "Admin", new DateTime(2025, 5, 29, 7, 36, 51, 83, DateTimeKind.Utc).AddTicks(5255) }
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_comment_reports_comment_id",
                table: "comment_reports",
                column: "comment_id");

            _ = migrationBuilder.CreateIndex(
                name: "IX_comment_reports_reporter_id",
                table: "comment_reports",
                column: "reporter_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "comment_reports");

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("50deb9c7-b5f2-41d5-a79a-cef55b29cfce"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("f2bf4ed5-9634-4c60-b62a-e4b5cf9c2df3"));

            _ = migrationBuilder.DropColumn(
                name: "status",
                table: "comments");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4d2b901b-cc8b-467d-962c-574a5be0479b"), new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2855), false, "User", new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2856) },
                    { new Guid("cf114091-2437-4c17-8148-ed251cf7e2f0"), new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2852), false, "Admin", new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2853) }
                });
        }
    }
}
