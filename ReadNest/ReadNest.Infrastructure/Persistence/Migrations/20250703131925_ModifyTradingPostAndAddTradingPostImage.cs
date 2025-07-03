using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ReadNest.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ModifyTradingPostAndAddTradingPostImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("ab6374cb-29f7-468f-8977-7fabcd66f797"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("ca4c94d5-40f9-4c2a-b246-5f506ff68467"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("103c89d3-f6a8-4dbd-882d-8b1d3838f4e0"), new DateTime(2025, 7, 3, 13, 19, 24, 452, DateTimeKind.Utc).AddTicks(7487), false, "Admin", new DateTime(2025, 7, 3, 13, 19, 24, 452, DateTimeKind.Utc).AddTicks(7489) },
                    { new Guid("7254a2cc-f593-4472-87cb-079500544116"), new DateTime(2025, 7, 3, 13, 19, 24, 452, DateTimeKind.Utc).AddTicks(7492), false, "User", new DateTime(2025, 7, 3, 13, 19, 24, 452, DateTimeKind.Utc).AddTicks(7492) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("103c89d3-f6a8-4dbd-882d-8b1d3838f4e0"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("7254a2cc-f593-4472-87cb-079500544116"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "role_name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("ab6374cb-29f7-468f-8977-7fabcd66f797"), new DateTime(2025, 6, 19, 7, 16, 8, 446, DateTimeKind.Utc).AddTicks(7690), false, "Admin", new DateTime(2025, 6, 22, 3, 56, 10, 737, DateTimeKind.Utc).AddTicks(6184) },
                    { new Guid("ca4c94d5-40f9-4c2a-b246-5f506ff68467"), new DateTime(2025, 6, 22, 3, 56, 10, 737, DateTimeKind.Utc).AddTicks(6186), false, "User", new DateTime(2025, 6, 22, 3, 56, 10, 737, DateTimeKind.Utc).AddTicks(6186) }
                });
        }
    }
}
