using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldBioAndBookImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("2bdbf018-3e48-4119-ace9-c3f2a84d4749"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("a971c86f-8e8d-44a9-980c-1564dc983f09"));

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "book_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    book_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_book_images", x => x.id);
                    table.ForeignKey(
                        name: "fk_book_images_book_id",
                        column: x => x.book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("55de61d6-6c12-4ec6-9600-e7b1df3a8f91"), new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5336), false, "User", new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5337) },
                    { new Guid("b1b59770-a2de-4127-8c20-b1e1e20a1b82"), new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5331), false, "Admin", new DateTime(2025, 5, 21, 16, 9, 2, 686, DateTimeKind.Utc).AddTicks(5333) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_book_images_book_id",
                table: "book_images",
                column: "book_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "book_images");

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("55de61d6-6c12-4ec6-9600-e7b1df3a8f91"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("b1b59770-a2de-4127-8c20-b1e1e20a1b82"));

            migrationBuilder.DropColumn(
                name: "bio",
                table: "users");

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2bdbf018-3e48-4119-ace9-c3f2a84d4749"), new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2803), false, "Admin", new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2806) },
                    { new Guid("a971c86f-8e8d-44a9-980c-1564dc983f09"), new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2808), false, "User", new DateTime(2025, 5, 17, 16, 29, 28, 110, DateTimeKind.Utc).AddTicks(2809) }
                });
        }
    }
}
