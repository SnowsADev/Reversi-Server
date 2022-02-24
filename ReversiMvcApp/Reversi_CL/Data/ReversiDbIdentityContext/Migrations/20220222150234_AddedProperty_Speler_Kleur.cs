using Microsoft.EntityFrameworkCore.Migrations;

namespace Reversi_CL.Data.ReversiDbIdentityContext.Migrations
{
    public partial class AddedProperty_Speler_Kleur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kleur",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kleur",
                table: "AspNetUsers");
        }
    }
}
