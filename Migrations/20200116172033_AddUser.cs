using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharesAPI.Migrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Wallet = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "StockOwnership",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    StockName = table.Column<string>(nullable: true),
                    AmountOwned = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockOwnership", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockOwnership_Users_Username",
                        column: x => x.Username,
                        principalTable: "Users",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockOwnership_Username",
                table: "StockOwnership",
                column: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockOwnership");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
