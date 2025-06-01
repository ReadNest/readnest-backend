using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsNormalizedBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("55de61d6-6c12-4ec6-9600-e7b1df3a8f91"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("b1b59770-a2de-4127-8c20-b1e1e20a1b82"));

            _ = migrationBuilder.AddColumn<string>(
                name: "author_normalized",
                table: "books",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "description_normalized",
                table: "books",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "title_normalized",
                table: "books",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4d2b901b-cc8b-467d-962c-574a5be0479b"), new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2855), false, "User", new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2856) },
                    { new Guid("cf114091-2437-4c17-8148-ed251cf7e2f0"), new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2852), false, "Admin", new DateTime(2025, 5, 25, 5, 16, 34, 102, DateTimeKind.Utc).AddTicks(2853) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("4d2b901b-cc8b-467d-962c-574a5be0479b"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cf114091-2437-4c17-8148-ed251cf7e2f0"));

            _ = migrationBuilder.DropColumn(
                name: "author_normalized",
                table: "books");

            _ = migrationBuilder.DropColumn(
                name: "description_normalized",
                table: "books");

            _ = migrationBuilder.DropColumn(
                name: "title_normalized",
                table: "books");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("55de61d6-6c12-4ec6-9600-e7b1df3a8f91"), new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5336), false, "User", new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5337) },
                    { new Guid("b1b59770-a2de-4127-8c20-b1e1e20a1b82"), new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5331), false, "Admin", new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5333) }
                });
        }
    }
}
