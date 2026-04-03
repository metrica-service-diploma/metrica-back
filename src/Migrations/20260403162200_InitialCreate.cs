using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace metrica_back.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TrackingCode = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Domain = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Websites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "UserName" },
                values: new object[] { new Guid("5d018eb5-f880-42ac-a94b-f93353876657"), new DateTime(2026, 3, 4, 16, 21, 58, 160, DateTimeKind.Utc).AddTicks(7928), "john.doe@example.com", "$2a$11$cSAFA9SmdNo5ZuCrtGORmOA01ci/io7InsH3op7RlEVBL0msBJwyG", "john_doe" });

            migrationBuilder.InsertData(
                table: "Websites",
                columns: new[] { "Id", "CreatedAt", "Domain", "Name", "TrackingCode", "UserId" },
                values: new object[,]
                {
                    { new Guid("27187291-8a48-4b5a-8e3a-5781b14f256b"), new DateTime(2026, 3, 30, 16, 21, 58, 496, DateTimeKind.Utc).AddTicks(1652), "blog.example.com", "Блог", 2, new Guid("5d018eb5-f880-42ac-a94b-f93353876657") },
                    { new Guid("a5911306-77a3-40cd-8729-5b7b68d125b5"), new DateTime(2026, 3, 29, 16, 21, 58, 496, DateTimeKind.Utc).AddTicks(1155), "shop.example.com", "Интернет-магазин", 1, new Guid("5d018eb5-f880-42ac-a94b-f93353876657") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Websites_UserId",
                table: "Websites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Websites");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
