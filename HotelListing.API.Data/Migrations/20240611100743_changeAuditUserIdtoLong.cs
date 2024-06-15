using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelListing.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeAuditUserIdtoLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "AuditUser",
                table: "AuditLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AuditUser",
                table: "AuditLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationTime",
                value: new DateTime(2024, 6, 11, 17, 49, 39, 367, DateTimeKind.Local).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationTime",
                value: new DateTime(2024, 6, 11, 17, 49, 39, 367, DateTimeKind.Local).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationTime",
                value: new DateTime(2024, 6, 11, 17, 49, 39, 367, DateTimeKind.Local).AddTicks(9656));
        }
    }
}
