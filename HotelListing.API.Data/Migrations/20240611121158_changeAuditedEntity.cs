using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeAuditedEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "Hotels",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "Hotels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "Hotels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Hotels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Hotels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "Hotels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "Hotels",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreationTime", "CreatorUserId", "DeleterUserId", "DeletionTime", "IsDeleted", "LastModificationTime", "LastModifierUserId" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, false, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "Hotels");

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2024, 6, 11, 18, 7, 41, 82, DateTimeKind.Local).AddTicks(4029));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2024, 6, 11, 18, 7, 41, 82, DateTimeKind.Local).AddTicks(4052));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(2024, 6, 11, 18, 7, 41, 82, DateTimeKind.Local).AddTicks(4055));
        }
    }
}
