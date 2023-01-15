using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HSForumAPI.Infrastructure.Migrations
{
    public partial class AddedIsActiveToPostReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PostReplies",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "PostTypes",
                columns: new[] { "PostTypeId", "Type" },
                values: new object[] { 2, "Tech_help" });

            migrationBuilder.InsertData(
                table: "PostTypes",
                columns: new[] { "PostTypeId", "Type" },
                values: new object[] { 3, "Intel" });

            migrationBuilder.InsertData(
                table: "PostTypes",
                columns: new[] { "PostTypeId", "Type" },
                values: new object[] { 4, "AMD" });

            migrationBuilder.InsertData(
                table: "PostTypes",
                columns: new[] { "PostTypeId", "Type" },
                values: new object[] { 5, "Nvidia" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 1, 15, 2, 8, 54, 611, DateTimeKind.Local).AddTicks(501), new byte[] { 248, 51, 5, 11, 173, 94, 175, 198, 242, 54, 48, 190, 71, 241, 15, 30, 22, 65, 117, 8, 139, 141, 30, 152, 105, 47, 76, 215, 170, 150, 74, 231 }, new byte[] { 170, 94, 209, 136, 42, 86, 3, 111, 239, 132, 112, 39, 182, 160, 37, 54, 238, 230, 208, 220, 174, 241, 0, 145, 141, 22, 126, 171, 179, 48, 111, 186, 45, 244, 213, 100, 68, 180, 240, 34, 126, 61, 117, 53, 17, 252, 86, 169, 5, 22, 167, 162, 91, 226, 32, 53, 85, 171, 157, 210, 71, 64, 193, 38 } });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "PostTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "PostTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "PostTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PostTypes",
                keyColumn: "PostTypeId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PostReplies");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2023, 1, 14, 15, 40, 11, 342, DateTimeKind.Local).AddTicks(6240), new byte[] { 18, 34, 181, 66, 55, 217, 63, 31, 36, 99, 199, 97, 124, 53, 22, 244, 70, 61, 99, 43, 97, 105, 220, 178, 65, 54, 131, 242, 176, 128, 1, 164 }, new byte[] { 59, 178, 26, 240, 147, 28, 10, 42, 13, 31, 82, 129, 11, 249, 114, 142, 206, 68, 65, 227, 111, 201, 205, 70, 97, 242, 17, 154, 193, 182, 171, 29, 85, 231, 18, 130, 148, 103, 105, 11, 52, 195, 131, 12, 251, 230, 117, 131, 95, 135, 191, 187, 167, 69, 222, 1, 30, 252, 55, 182, 183, 97, 21, 79 } });
        }
    }
}
