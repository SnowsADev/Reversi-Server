using Microsoft.EntityFrameworkCore.Migrations;

namespace Reversi_CL.Data.ReversiDbIdentityContext.Migrations
{
    public partial class SeedData_Users_Roles_UserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44f80e14-9ca1-48db-b6cc-2a3153d01657", "a7c919cc-b5c8-4702-91ce-94fdb51762fe", "Mediator", "MEDIATOR" },
                    { "512a1ac9-a17e-4602-a200-bf9e0ac10a6c", "b8aca75b-d513-4eab-b374-bb84cc7acafe", "Admin", "ADMIN" },
                    { "d4297ca9-77fe-496f-a148-7c7232811f85", "a7d902e5-96c6-4985-8004-c6701ada706c", "Speler", "SPELER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AantalGelijk", "AantalGewonnen", "AantalVerloren", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsEnabled", "Kleur", "LockoutEnabled", "LockoutEnd", "Naam", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Spel", "SpelID", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "60c35514-e6cf-47f4-84ad-8b010ce0b7b4", 0, 0, 0, 0, "4b36a5cc-3022-42a2-81da-74866c69d0b6", "admin@reversi.nl", true, true, 0, false, null, "Admin Bram", "ADMIN@REVERSI.NL", null, "AQAAAAEAACcQAAAAEHYggDbPjVt3bc3HHLkCpcc0SXMKwlwnmJmhGZxFqb681ce6tocmGk3en3L5iQNdjg==", "12345678", false, "0517346c-8db1-441d-904f-61d6d7ed4881", null, null, false, "admin@reversi.nl" },
                    { "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7", 0, 0, 0, 0, "dcc2b7bf-a4ad-48a8-bcab-ae9532104d41", "mediator@reversi.nl", true, true, 0, false, null, "Mediator Bram", "MEDIATOR@REVERSI.NL", null, "AQAAAAEAACcQAAAAEJWTSg+cmFGMA37KKkYxZea4SR1QOv6999S0x7WpJ20dh9p6MY3u2qHXIXeZRE4TQg==", "12345678", false, "36408ffa-ab1a-4122-b762-b8e3662798d6", null, null, false, "mediator@reversi.nl" },
                    { "c7d9dd9f-99bc-422f-8f7f-0828c45acf68", 0, 0, 0, 0, "6a2e7ed7-5eab-44a2-89f1-8f1b5ed17057", "speler@reversi.nl", true, true, 0, false, null, "Bram", "SPELER@REVERSI.NL", null, "AQAAAAEAACcQAAAAENRMKds7/9Kc0jQjUIOTLO3ZTgN2MGJHM6GLXEmhOHvWsgFzN6w0dqmlPAAYH5QuvA==", "12345678", false, "b28d0fa0-4f8a-456b-944c-bf2a61a96fda", null, null, false, "speler@reversi.nl" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "60c35514-e6cf-47f4-84ad-8b010ce0b7b4", "512a1ac9-a17e-4602-a200-bf9e0ac10a6c" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7", "44f80e14-9ca1-48db-b6cc-2a3153d01657" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { "c7d9dd9f-99bc-422f-8f7f-0828c45acf68", "d4297ca9-77fe-496f-a148-7c7232811f85" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "60c35514-e6cf-47f4-84ad-8b010ce0b7b4", "512a1ac9-a17e-4602-a200-bf9e0ac10a6c" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7", "44f80e14-9ca1-48db-b6cc-2a3153d01657" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { "c7d9dd9f-99bc-422f-8f7f-0828c45acf68", "d4297ca9-77fe-496f-a148-7c7232811f85" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44f80e14-9ca1-48db-b6cc-2a3153d01657");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "512a1ac9-a17e-4602-a200-bf9e0ac10a6c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4297ca9-77fe-496f-a148-7c7232811f85");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "60c35514-e6cf-47f4-84ad-8b010ce0b7b4");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c7d9dd9f-99bc-422f-8f7f-0828c45acf68");
        }
    }
}
