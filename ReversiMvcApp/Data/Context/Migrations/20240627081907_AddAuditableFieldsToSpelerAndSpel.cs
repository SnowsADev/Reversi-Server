using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReversiMvcApp.Data.Context.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableFieldsToSpelerAndSpel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Spellen");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Spellen",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Spellen",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Spellen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "Spellen",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44f80e14-9ca1-48db-b6cc-2a3153d01657",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "512a1ac9-a17e-4602-a200-bf9e0ac10a6c",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4297ca9-77fe-496f-a148-7c7232811f85",
                column: "ConcurrencyStamp",
                value: null);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "60c35514-e6cf-47f4-84ad-8b010ce0b7b4",
                columns: new[] { "ConcurrencyStamp", "Created", "CreatedOn", "LastUpdated", "Modified", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2cf65307-59e0-4a98-8fca-d5b75f8ab0ed", null, new DateTime(2024, 6, 27, 8, 19, 6, 488, DateTimeKind.Utc).AddTicks(2324), new DateTime(2024, 6, 27, 8, 19, 6, 488, DateTimeKind.Utc).AddTicks(2327), null, "AQAAAAIAAYagAAAAEKq4qN1FpxAAB7y/SFwHjMjSAKOmxwzddQAP3icZMvffDRL0oj7eZcXHJfdNlu4jEA==", "1db604ad-61a4-4db9-ab99-31c7ce9d860c" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7",
                columns: new[] { "ConcurrencyStamp", "Created", "CreatedOn", "LastUpdated", "Modified", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cd5c3e8d-c22c-4aee-8be9-273bc1ba9c34", null, new DateTime(2024, 6, 27, 8, 19, 6, 586, DateTimeKind.Utc).AddTicks(1517), new DateTime(2024, 6, 27, 8, 19, 6, 586, DateTimeKind.Utc).AddTicks(1524), null, "AQAAAAIAAYagAAAAEN9RJ6QnIoLXMZ0KbZ8yOY3X71WgMaLzoCeKVoIC+802MYQ9Gpx1BEBA9l9FXIk7CQ==", "ac83e299-0bb7-4dc1-a8b7-49077a6fc53d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c7d9dd9f-99bc-422f-8f7f-0828c45acf68",
                columns: new[] { "ConcurrencyStamp", "Created", "CreatedOn", "LastUpdated", "Modified", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b9cf5d5-098b-4e8e-b0d2-51feaa98261a", null, new DateTime(2024, 6, 27, 8, 19, 6, 672, DateTimeKind.Utc).AddTicks(3616), new DateTime(2024, 6, 27, 8, 19, 6, 672, DateTimeKind.Utc).AddTicks(3626), null, "AQAAAAIAAYagAAAAEA9Nf+nYAJsl0BuuHbMXgRJAnWsA6ZGdNMrAkcJd+Xa8UFLT/N97MM7E2YVTXy38OA==", "a664cade-09b1-4502-8434-15c5f8b13b99" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "Spellen");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "Spellen");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdated",
                table: "Spellen",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Spellen",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Spellen",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44f80e14-9ca1-48db-b6cc-2a3153d01657",
                column: "ConcurrencyStamp",
                value: "2535d321-ed90-449b-9083-b4a90d24bc3f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "512a1ac9-a17e-4602-a200-bf9e0ac10a6c",
                column: "ConcurrencyStamp",
                value: "07abf234-536c-4a93-898f-0b38587b837c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d4297ca9-77fe-496f-a148-7c7232811f85",
                column: "ConcurrencyStamp",
                value: "c4b8dcc6-c2d6-4ec3-b3df-88cbbb5214fd");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "60c35514-e6cf-47f4-84ad-8b010ce0b7b4",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cb8fa02e-0ae1-4a88-80f9-ac04a6abd211", "AQAAAAEAACcQAAAAEKj18Pf5Qm42qZ6lknl71O3FPg7O/7E21XofCsDrxShUjnNHDuj7wwirRHHX8qwFww==", "dd75956b-ae53-44ac-92a1-3bb7a2d5afe6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a5b07a52-c6e0-4dab-8a28-42c038e0cbb7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "607f68fd-c9f6-4a2a-96e6-acd6e8ba20e8", "AQAAAAEAACcQAAAAEOELcPa97IPKD08NZmijJGvoi4WOfC25++tSc1mVtkxYliy0vCk0zDg8gcOnr9yIOQ==", "8adb6479-5e67-444e-9f17-4e7b2b0307c1" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c7d9dd9f-99bc-422f-8f7f-0828c45acf68",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "89b74710-7f15-4283-9eff-1f584ef60159", "AQAAAAEAACcQAAAAELp2bJ2SJe+HUTmCKkgE4oPo14NEgSoapEORNqySmzhLpiD2LwV8+YAQKa7fWyiNYQ==", "09022a54-359a-4ad3-a4b3-bfee38639a34" });
        }
    }
}
