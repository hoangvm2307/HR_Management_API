using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class connectuseriwthuserinfor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfor_userId_userId",
                table: "UserInfor");

            migrationBuilder.DropIndex(
                name: "IX_UserInfor_userId",
                table: "UserInfor");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "53ddb440-038a-4a5d-b9bd-36071d31755b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7cd25b4d-cdb3-49d2-9ba6-9aeec5a6f7db");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "912237ef-e9be-445d-a775-cb5aee8e07f4");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "UserInfor",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserAccountUserId",
                table: "UserInfor",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "117d42d0-cd99-44ee-8e82-0f130c84f1f3", null, "Staff", "STAFF" },
                    { "bf565746-b373-4431-8345-289a56052485", null, "HRManager", "HRMANAGER" },
                    { "e7026f67-4a03-4498-8dde-2cc35874a9a1", null, "HRStaff", "HRSTAFF" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfor_UserAccountUserId",
                table: "UserInfor",
                column: "UserAccountUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInfor_userId",
                table: "UserInfor",
                column: "userId",
                unique: true,
                filter: "[userId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfor_UserAccount_UserAccountUserId",
                table: "UserInfor",
                column: "UserAccountUserId",
                principalTable: "UserAccount",
                principalColumn: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfor_userId_userId",
                table: "UserInfor",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInfor_UserAccount_UserAccountUserId",
                table: "UserInfor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfor_userId_userId",
                table: "UserInfor");

            migrationBuilder.DropIndex(
                name: "IX_UserInfor_UserAccountUserId",
                table: "UserInfor");

            migrationBuilder.DropIndex(
                name: "IX_UserInfor_userId",
                table: "UserInfor");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "117d42d0-cd99-44ee-8e82-0f130c84f1f3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bf565746-b373-4431-8345-289a56052485");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e7026f67-4a03-4498-8dde-2cc35874a9a1");

            migrationBuilder.DropColumn(
                name: "UserAccountUserId",
                table: "UserInfor");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "UserInfor",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "53ddb440-038a-4a5d-b9bd-36071d31755b", null, "HRStaff", "HRSTAFF" },
                    { "7cd25b4d-cdb3-49d2-9ba6-9aeec5a6f7db", null, "Staff", "STAFF" },
                    { "912237ef-e9be-445d-a775-cb5aee8e07f4", null, "HRManager", "HRMANAGER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfor_userId",
                table: "UserInfor",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfor_userId_userId",
                table: "UserInfor",
                column: "userId",
                principalTable: "UserAccount",
                principalColumn: "userId");
        }
    }
}
