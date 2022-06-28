using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesTest.Migrations
{
    public partial class adminseeder4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "id",
                keyValue: 2,
                column: "password",
                value: "1UzrWHA24VbkJZO+JMC9EDNX92/fgrKOBKd0Vq0F0ho=");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "id",
                keyValue: 2,
                column: "password",
                value: "ydYX/HOdx0/7OWqJ8+YTaFHilW+jKO01U3BIyXkagJ4=");
        }
    }
}
