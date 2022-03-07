﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Reversi_CL.Data.ReversiDbContext.Migrations
{
    public partial class Added_Spel_Afgelopen_Bool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Afgelopen",
                table: "Spellen",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Afgelopen",
                table: "Spellen");
        }
    }
}