using Microsoft.EntityFrameworkCore.Migrations;

namespace Reversi_CL.Data.ReversiDbIdentityContext.Migrations
{
    public partial class Accounts_IsEnabled_Property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Spel");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Spel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
