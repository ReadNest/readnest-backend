using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldAvatarUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("0c2d0b97-0520-4af3-ab0a-211295f8192a"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("9d5ac316-0291-4983-a765-4ef7d9e64bf6"));

            _ = migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("057b3ea4-2643-4f47-a1d8-27e3d06e75a6"), new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4025), false, "Admin", new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4028) },
                    { new Guid("7d7f547b-72cf-4aca-a93a-f50456b17c9d"), new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4029), false, "User", new DateTime(2025, 5, 15, 23, 44, 38, 410, DateTimeKind.Utc).AddTicks(4030) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("057b3ea4-2643-4f47-a1d8-27e3d06e75a6"));

            _ = migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("7d7f547b-72cf-4aca-a93a-f50456b17c9d"));

            _ = migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "users");

            _ = migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0c2d0b97-0520-4af3-ab0a-211295f8192a"), new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6094), false, "User", new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6095) },
                    { new Guid("9d5ac316-0291-4983-a765-4ef7d9e64bf6"), new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6087), false, "Admin", new DateTime(2025, 5, 14, 1, 27, 50, 636, DateTimeKind.Utc).AddTicks(6093) }
                });
        }
    }
}
