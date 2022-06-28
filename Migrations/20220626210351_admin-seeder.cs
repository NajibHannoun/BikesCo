using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesTest.Migrations
{
    public partial class adminseeder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "id", "birthday", "email", "firstName", "lastName", "password", "username" },
                values: new object[] { 2, new DateTime(2000, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@admin.com", "admin", "admin", "1UzrWHA24VbkJZO+JMC9EDNX92/fgrKOBKd0Vq0F0ho=", "admin" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "id", "isCurrentlyLogged", "isSuspended", "user_id" },
                values: new object[] { 1, false, false, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "id",
                keyValue: 2);
        }
    }
}
