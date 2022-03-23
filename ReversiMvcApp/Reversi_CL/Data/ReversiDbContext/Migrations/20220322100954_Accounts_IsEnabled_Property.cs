using Microsoft.EntityFrameworkCore.Migrations;

namespace Reversi_CL.Data.ReversiDbContext.Migrations
{
    public partial class Accounts_IsEnabled_Property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Speler",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Speler");
        }
    }
}
