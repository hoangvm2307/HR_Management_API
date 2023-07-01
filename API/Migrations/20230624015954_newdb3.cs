using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class newdb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangeAt",
                table: "PersonnelContract",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "PersonnelContract",
                type: "datetime2",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1e0a9e37-8f5c-490e-b726-cc675e1f7fa4", null, "HRManager", "HRMANAGER" },
                    { "6e13be1e-d4cd-4243-8113-4785149fac93", null, "HRStaff", "HRSTAFF" },
                    { "956e1bff-aa1e-42ee-b03e-ae63b977b84f", null, "Staff", "STAFF" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1e0a9e37-8f5c-490e-b726-cc675e1f7fa4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e13be1e-d4cd-4243-8113-4785149fac93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "956e1bff-aa1e-42ee-b03e-ae63b977b84f");

            migrationBuilder.DropColumn(
                name: "ChangeAt",
                table: "PersonnelContract");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "PersonnelContract");

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
    }
}
