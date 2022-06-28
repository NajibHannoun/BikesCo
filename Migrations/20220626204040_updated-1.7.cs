using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikesTest.Migrations
{
    public partial class updated17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bicycles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    size = table.Column<int>(type: "int", nullable: false),
                    leassPrice = table.Column<double>(type: "float", nullable: false),
                    isCurrentlyRented = table.Column<bool>(type: "bit", nullable: false),
                    lastCheckupDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    timesRented = table.Column<int>(type: "int", nullable: false),
                    earningsToDate = table.Column<double>(type: "float", nullable: false),
                    aquisutionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    purchasePrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bicycles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    birthday = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isCurrentlyLogged = table.Column<bool>(type: "bit", nullable: false),
                    isSuspended = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    isCurrentlyBiking = table.Column<bool>(type: "bit", nullable: false),
                    timeBiked = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    numberOfBikesRented = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Customers_Users_user_id",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bicycle_Id = table.Column<int>(type: "int", nullable: false),
                    customer_Id = table.Column<int>(type: "int", nullable: false),
                    admin_Id = table.Column<int>(type: "int", nullable: false),
                    rentalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    returnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    durationOfTransaction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    transactionType = table.Column<int>(type: "int", nullable: false),
                    costOfTransaction = table.Column<double>(type: "float", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transactions_Admins_admin_Id",
                        column: x => x.admin_Id,
                        principalTable: "Admins",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transactions_Bicycles_bicycle_Id",
                        column: x => x.bicycle_Id,
                        principalTable: "Bicycles",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Transactions_Customers_customer_Id",
                        column: x => x.customer_Id,
                        principalTable: "Customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_user_id",
                table: "Admins",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_user_id",
                table: "Customers",
                column: "user_id",
                unique: true,
                filter: "[user_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_admin_Id",
                table: "Transactions",
                column: "admin_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_bicycle_Id",
                table: "Transactions",
                column: "bicycle_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_customer_Id",
                table: "Transactions",
                column: "customer_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Bicycles");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
