using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class newdb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "300aa559-caaf-4a31-8545-a8d2ebe9b5bb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "401f20ac-8cc8-45f9-8132-2d78ad1909e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8cf172f8-3542-44e2-b7d5-8692fc5d6e23");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "495a89fa-e956-49a7-b64a-cd7e467efc51", null, "HRStaff", "HRSTAFF" },
                    { "727dcdfb-209f-4626-a734-b5f5fc19bd0f", null, "Staff", "STAFF" },
                    { "ce0120d7-2682-4624-8081-f26b18c1ef48", null, "HRManager", "HRMANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "495a89fa-e956-49a7-b64a-cd7e467efc51");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "727dcdfb-209f-4626-a734-b5f5fc19bd0f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce0120d7-2682-4624-8081-f26b18c1ef48");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "300aa559-caaf-4a31-8545-a8d2ebe9b5bb", null, "HRStaff", "HRSTAFF" },
                    { "401f20ac-8cc8-45f9-8132-2d78ad1909e5", null, "Staff", "STAFF" },
                    { "8cf172f8-3542-44e2-b7d5-8692fc5d6e23", null, "HRManager", "HRMANAGER" }
                });
        }
    }
}
