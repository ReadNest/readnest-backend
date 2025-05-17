using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldLanguageAndISBN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("057b3ea4-2643-4f47-a1d8-27e3d06e75a6"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("7d7f547b-72cf-4aca-a93a-f50456b17c9d"));

            _ = migrationBuilder.AddColumn<string>(
                name: "isbn",
                table: "books",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.AddColumn<string>(
                name: "language",
                table: "books",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2bdbf018-3e48-4119-ace9-c3f2a84d4749"), new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2803), false, "Admin", new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2806) },
                    { new Guid("a971c86f-8e8d-44a9-980c-1564dc983f09"), new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2808), false, "User", new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2809) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("2bdbf018-3e48-4119-ace9-c3f2a84d4749"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("a971c86f-8e8d-44a9-980c-1564dc983f09"));

            _ = migrationBuilder.DropColumn(
                name: "isbn",
                table: "books");

            _ = migrationBuilder.DropColumn(
                name: "language",
                table: "books");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("057b3ea4-2643-4f47-a1d8-27e3d06e75a6"), new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4025), false, "Admin", new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4028) },
                    { new Guid("7d7f547b-72cf-4aca-a93a-f50456b17c9d"), new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4029), false, "User", new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4030) }
                });
        }
    }
}
