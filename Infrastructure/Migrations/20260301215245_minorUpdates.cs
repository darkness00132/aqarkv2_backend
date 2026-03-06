using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class minorUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityRole<Guid>");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0001"), "ROLE-USER-0001", "User", "USER" },
                    { new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0002"), "ROLE-ADMIN-0001", "Admin", "ADMIN" },
                    { new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0003"), "ROLE-SUPERADMIN-0001", "SuperAdmin", "SUPERADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0001"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0002"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0003"));

            migrationBuilder.CreateTable(
                name: "IdentityRole<Guid>",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    NormalizedName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityRole<Guid>", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "IdentityRole<Guid>",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0001"), "ROLE-USER-0001", "User", "USER" },
                    { new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0002"), "ROLE-ADMIN-0001", "Admin", "ADMIN" },
                    { new Guid("a8e2b5a2-2d7b-4f47-a1e0-2d8d2e9f0003"), "ROLE-SUPERADMIN-0001", "SuperAdmin", "SUPERADMIN" }
                });
        }
    }
}
