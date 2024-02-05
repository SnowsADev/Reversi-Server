using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ReversiMvcApp.Data.Context.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spellen",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AandeBeurt = table.Column<int>(nullable: false),
                    Omschrijving = table.Column<string>(nullable: true),
                    Afgelopen = table.Column<bool>(nullable: false),
                    Bord = table.Column<string>(type: "nvarchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spellen", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Naam = table.Column<string>(nullable: true),
                    AantalGewonnen = table.Column<int>(nullable: false),
                    AantalVerloren = table.Column<int>(nullable: false),
                    AantalGelijk = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Kleur = table.Column<int>(nullable: false),
                    Spel = table.Column<string>(nullable: true),
                    SpelID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Spellen_Spel",
                        column: x => x.Spel,
                        principalTable: "Spellen",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Spellen_SpelID",
                        column: x => x.SpelID,
                        principalTable: "Spellen",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44f80e14-9ca1-48db-b6cc-2a3153d01657", "2535d321-ed90-449b-9083-b4a90d24bc3f", "Mediator", "MEDIATOR" },
                    { "512a1ac9-a17e-4602-a200-bf9e0ac10a6c", "07abf234-536c-4a93-898f-0b38587b837c", "Admin", "ADMIN" },
                    { "d4297ca9-77fe-496f-a148-7c7232811f85", "c4b8dcc6-c2d6-4ec3-b3df-88cbbb5214fd", "Speler", "SPELER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AantalGelijk", "AantalGewonnen", "AantalVerloren", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsEnabled", "Kleur", "LockoutEnabled", "LockoutEnd", "Naam", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Spel", "SpelID", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "60c35514-e6cf-47f4-84ad-8b010ce0b7b4", 0, 0, 0, 0, "cb8fa02e-0ae1-4a88-80f9-ac04a6abd211", "admin@reversi.nl", true, true, 0, false, null, "Admin Bram", "ADMIN@REVERSI.NL", "ADMIN@REVERSI.NL", "AQAAAAEAACcQAAAAEKj18Pf5Qm42qZ6lknl71O3FPg7O/7E21XofCsDrxShUjnNHDuj7wwirRHHX8qwFww==", "12345678", false, "dd75956b-ae53-44ac-92a1-3bb7a2d5afe6", null, null, false, "admin@reversi.nl" },
                    { "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7", 0, 0, 0, 0, "607f68fd-c9f6-4a2a-96e6-acd6e8ba20e8", "mediator@reversi.nl", true, true, 0, false, null, "Mediator Bram", "MEDIATOR@REVERSI.NL", "MEDIATOR@REVERSI.NL", "AQAAAAEAACcQAAAAEOELcPa97IPKD08NZmijJGvoi4WOfC25++tSc1mVtkxYliy0vCk0zDg8gcOnr9yIOQ==", "12345678", false, "8adb6479-5e67-444e-9f17-4e7b2b0307c1", null, null, false, "mediator@reversi.nl" },
                    { "c7d9dd9f-99bc-422f-8f7f-0828c45acf68", 0, 0, 0, 0, "89b74710-7f15-4283-9eff-1f584ef60159", "speler@reversi.nl", true, true, 0, false, null, "Bram", "SPELER@REVERSI.NL", "SPELER@REVERSI.NL", "AQAAAAEAACcQAAAAELp2bJ2SJe+HUTmCKkgE4oPo14NEgSoapEORNqySmzhLpiD2LwV8+YAQKa7fWyiNYQ==", "12345678", false, "09022a54-359a-4ad3-a4b3-bfee38639a34", null, null, false, "speler@reversi.nl" }
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Spel",
                table: "AspNetUsers",
                column: "Spel");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpelID",
                table: "AspNetUsers",
                column: "SpelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Spellen");
        }
    }
}
