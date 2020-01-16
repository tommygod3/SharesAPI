using Microsoft.EntityFrameworkCore.Migrations;

namespace SharesAPI.Migrations
{
    public partial class AddOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOwnership_Users_Username",
                table: "StockOwnership");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockOwnership",
                table: "StockOwnership");

            migrationBuilder.DropColumn(
                name: "StockName",
                table: "StockOwnership");

            migrationBuilder.RenameTable(
                name: "StockOwnership",
                newName: "StockOwnerships");

            migrationBuilder.RenameIndex(
                name: "IX_StockOwnership_Username",
                table: "StockOwnerships",
                newName: "IX_StockOwnerships_Username");

            migrationBuilder.AlterColumn<int>(
                name: "AmountOwned",
                table: "StockOwnerships",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "StockOwnerships",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockOwnerships",
                table: "StockOwnerships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockOwnerships_Users_Username",
                table: "StockOwnerships",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockOwnerships_Users_Username",
                table: "StockOwnerships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockOwnerships",
                table: "StockOwnerships");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "StockOwnerships");

            migrationBuilder.RenameTable(
                name: "StockOwnerships",
                newName: "StockOwnership");

            migrationBuilder.RenameIndex(
                name: "IX_StockOwnerships_Username",
                table: "StockOwnership",
                newName: "IX_StockOwnership_Username");

            migrationBuilder.AlterColumn<string>(
                name: "AmountOwned",
                table: "StockOwnership",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "StockName",
                table: "StockOwnership",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockOwnership",
                table: "StockOwnership",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockOwnership_Users_Username",
                table: "StockOwnership",
                column: "Username",
                principalTable: "Users",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
